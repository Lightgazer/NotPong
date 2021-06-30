using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NotPong
{
    internal class BombBonus : Bonus
    {
        private double detonateTime = 250;

        public override void Update(GameTime gameTime, GameGrid grid)
        {
            if (Active)
            {
                detonateTime -= gameTime.ElapsedGameTime.TotalMilliseconds;
                if (detonateTime < 0)
                {
                    Active = false;

                    for (var x = -1; x <= 1; x++)
                    {
                        for (var y = -1; y <= 1; y++)
                        {
                            var doomIndex = new Point(index.X + x, index.Y + y);
                            grid.TryKillBlock(doomIndex);
                        }
                    }
                }
            }
        }
        
        public override void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(Texture, position, null, Color.White, 0f, Block.Origin, 1, SpriteEffects.None, 0f);
        }
    }
}
