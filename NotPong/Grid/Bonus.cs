using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NotPong
{
    class Bonus
    {
        public Texture2D BonusTexture { get; set; }

        public void Draw(SpriteBatch spriteBatch, Vector2 position) {
            spriteBatch.Draw(BonusTexture, position, null, Color.White, 0f, Block.origin, 1, SpriteEffects.None, 0f);
        }
    }
}
