using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NotPong
{
    internal class LineBonus : Bonus
    {
        public bool vertical;
        private float effectPosition = 0;

        public override void Update(GameTime gameTime)
        {
            if (Active)
            {
                effectPosition += (float)gameTime.ElapsedGameTime.TotalSeconds * GameSettings.BlockSize * 4 * GameSettings.AnimationSpeed;
                var stepsToStart = vertical ? index.X : index.Y;
                var stepsToEnd = GameSettings.GridSize - stepsToStart;
                var stepsNeeded = stepsToStart > stepsToEnd ? stepsToStart : stepsToEnd;

                var effectIndex = (int)(effectPosition / GameSettings.BlockSize);
                var forwardIndex = vertical ? new Point(index.X + effectIndex, index.Y) : new Point(index.X, index.Y + effectIndex);
                if (IsIndexInBounds(forwardIndex))
                    FireBlock(forwardIndex);

                var backwardIndex = vertical ? new Point (index.X - effectIndex, index.Y) : new Point (index.X, index.Y - effectIndex);
                if (IsIndexInBounds(backwardIndex))
                    FireBlock(backwardIndex);


                if (effectPosition / GameSettings.BlockSize > stepsNeeded)
                    Active = false;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            var rotation = vertical ? 1.57f : 0f;
            spriteBatch.Draw(Texture, position, null, Color.White, rotation, Block.origin, 1, SpriteEffects.None, 0f);
            if (Active)
            {
                var vector = vertical ? new Vector2(effectPosition, 0) : new Vector2(0, effectPosition);
                spriteBatch.Draw(Texture, position + vector, null, Color.White, rotation, Block.origin, 1, SpriteEffects.None, 0f);
                spriteBatch.Draw(Texture, position - vector, null, Color.White, rotation, Block.origin, 1, SpriteEffects.None, 0f);
            }
        }
    }
}
