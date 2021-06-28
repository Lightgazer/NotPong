using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;

namespace NotPong
{
    internal enum BlockState
    {
        ///<summary>Двигается, либо падение, либо свап</summary>
        Moving,

        ///<summary>Свапнутый блок, ждёт проверку на матч, затем возвращется или умирает</summary>
        Suspect,

        ///<summary>Умер, постепенно исчезает</summary>
        Dead,

        ///<summary>Разложился, можно падать на его место</summary>
        Rotten,

        ///<summary>Ожидает, если все блоки ожидают игрок может сделать свап</summary>
        Idle
    }

    internal class Block
    {
        private const float RottenSize = 0.2f;
        
        public readonly int type;
        public BlockState state = BlockState.Idle;
        public Bonus Bonus { get; set; }
        public Bonus NextBonus { get; set; }
        private float Size { get; set; } = 1;
        private Vector2 MovementDisplacement { get; set; } = new Vector2(0, 0);
        public bool morgueTiket = false;
        public bool crossingFlag = false;
        public static Vector2 origin = new Vector2(GameSettings.BlockSize / 2);

        private readonly Texture2D texture;
        private BlockState lastState;

        public Block(int type, Texture2D texture)
        {
            this.type = type;
            this.texture = texture;
        }

        public void ActivateBonus(Block[,] grid)
        {
            var index = grid.Cast<Block>().ToList().FindIndex(block => block == this);
            if (!IsBonusActive())
                Bonus?.Activate(new Point(index / GameSettings.GridSize, index % GameSettings.GridSize));
        }

        public bool IsBonusActive()
        {
            return Bonus is {Active: true};
        }

        public void MoveFrom(Vector2 direction)
        {
            if (state == BlockState.Moving) return;
            direction.Normalize();
            MovementDisplacement = direction * GameSettings.BlockSize;
            lastState = state;
            state = BlockState.Moving;
        }

        public void Update(GameTime gameTime, GameGrid parent)
        {
            ManageSize(gameTime);
            ManageMovement(gameTime);
            Bonus?.Update(gameTime, parent);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            position += origin + MovementDisplacement;
            spriteBatch.Draw(texture, position, null, Color.White, 0f, origin, Size, SpriteEffects.None, 0f);
        }

        public void DrawBonus(SpriteBatch spriteBatch, Vector2 position)
        {
            position += origin + MovementDisplacement;
            Bonus?.Draw(spriteBatch, position);
        }
        
        private void ManageMovement(GameTime gameTime)
        {
            if (state == BlockState.Moving)
            {
                var delta = (float) gameTime.ElapsedGameTime.TotalSeconds * GameSettings.BlockSize * 2 *
                            GameSettings.AnimationSpeed;
                MovementDisplacement = MyMath.MoveTowards(MovementDisplacement, new Vector2(0), delta);
                if (MovementDisplacement == new Vector2(0))
                {
                    state = lastState;
                }
            }
        }

        private void ManageSize(GameTime gameTime)
        {
            var targetSize = state == BlockState.Dead ? RottenSize : 1f;

            var delta = (float) gameTime.ElapsedGameTime.TotalSeconds * GameSettings.AnimationSpeed;
            Size = MyMath.MoveTowards(Size, targetSize, delta);
            if (Math.Abs(Size - RottenSize) < float.Epsilon && !IsBonusActive())
                DeathTrial();
        }

        private void DeathTrial()
        {
            if (NextBonus is { })
            {
                Bonus = NextBonus;
                NextBonus = null;
                state = BlockState.Idle;
                return;
            }

            state = BlockState.Rotten;
        }
    }
}