using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NotPong
{
    enum Direction
    {
        Up,
        Down,
        Left,
        Rigth
    }

    enum BlockState
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

    class Block
    {
        public int type;
        public BlockState state = BlockState.Idle;
        public float Size { get; private set; } = 1;
        public Vector2 MovementDisplacement { get; private set; } = new Vector2(0, 0);
        public Bonus Bonus { get; set; }

        public static Vector2 origin = new Vector2(GameSettings.blockSize / 2);
        protected BlockState lastState;
        protected Texture2D texture;


        public Block(int type, Texture2D texture)
        {
            this.type = type;
            this.texture = texture;
        }

        public void Fire()
        {

        }

        public void MoveFrom(Vector2 direction)
        {
            if (state == BlockState.Moving) return;
            direction.Normalize();
            MovementDisplacement = direction * GameSettings.blockSize;
            lastState = state;
            state = BlockState.Moving;
        }

        public void Update(GameTime gameTime)
        {
            if (state == BlockState.Dead)
            {
                Size = MyMath.MoveTowards(Size, 0.2f, (float)gameTime.ElapsedGameTime.TotalSeconds * GameSettings.animationSpeed);
                if (Size == 0.2f) state = BlockState.Rotten;
            }

            if (state == BlockState.Moving)
            {
                MovementDisplacement = MyMath.MoveTowards(MovementDisplacement, new Vector2(0), (float)gameTime.ElapsedGameTime.TotalSeconds * GameSettings.blockSize * 2 * GameSettings.animationSpeed);
                if (MovementDisplacement == new Vector2(0))
                {
                    state = lastState;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            position += origin;
            position += MovementDisplacement;
            spriteBatch.Draw(texture, position, null, Color.White, 0f, origin, Size, SpriteEffects.None, 0f);
            Bonus?.Draw(spriteBatch, position);
        }
    }
}
