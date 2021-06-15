using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NotPong
{
    class BombBonus : Bonus
    {
        private double detonateTime = 250;

        public override void Update(GameTime gameTime)
        {
            if (Active)
            {
                detonateTime -= gameTime.ElapsedGameTime.TotalMilliseconds;
                if (detonateTime < 0)
                {
                    Active = false;

                    for (int x = -1; x <= 1; x++)
                    {
                        for (int y = -1; y <= 1; y++)
                        {
                            var doomIndex = new Point(index.X + x, index.Y + y);
                            if (IsIndexInBounds(doomIndex))
                                FireBlock(doomIndex);
                        }
                    }
                }
            }
        }
        public override void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(BonusTexture, position, null, Color.White, 0f, Block.origin, 1, SpriteEffects.None, 0f);
        }
    }
}
