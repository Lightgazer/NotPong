using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NotPong
{
    class LineBonus : Bonus
    {
        public bool vertical;
        private float effectPosition = 0;

        public override void Update(GameTime gameTime)
        {
            if (Active)
            {
                effectPosition += (float)gameTime.ElapsedGameTime.TotalSeconds * GameSettings.blockSize * 4 * GameSettings.animationSpeed;
                var stepsToStart = vertical ? index.X : index.Y;
                var stepsToEnd = GameSettings.gridSize - stepsToStart;
                var stepsNeeded = stepsToStart > stepsToEnd ? stepsToStart : stepsToEnd;

                var effectIndex = (int)(effectPosition / GameSettings.blockSize);
                var fowardIndex = vertical ? new Point(index.X + effectIndex, index.Y) : new Point(index.X, index.Y + effectIndex);
                if (IsIndexInBounds(fowardIndex))
                    FireBlock(fowardIndex);

                var backwardIndex = vertical ? new Point (index.X - effectIndex, index.Y) : new Point (index.X, index.Y - effectIndex);
                if (IsIndexInBounds(backwardIndex))
                    FireBlock(backwardIndex);


                if (effectPosition / GameSettings.blockSize > stepsNeeded)
                    Active = false;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            var rotation = vertical ? 1.57f : 0f;
            spriteBatch.Draw(BonusTexture, position, null, Color.White, rotation, Block.origin, 1, SpriteEffects.None, 0f);
            if (Active)
            {
                var vector = vertical ? new Vector2(effectPosition, 0) : new Vector2(0, effectPosition);
                spriteBatch.Draw(BonusTexture, position + vector, null, Color.White, rotation, Block.origin, 1, SpriteEffects.None, 0f);
                spriteBatch.Draw(BonusTexture, position - vector, null, Color.White, rotation, Block.origin, 1, SpriteEffects.None, 0f);
            }
        }


    }
}
