using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NotPong
{
    internal class ScoreWidget
    {
        public int Score { private get; set; } = 0;
        private readonly Vector2 position = new Vector2((GameSettings.Width - GameSettings.GridSize * GameSettings.BlockSize) / 2, 10);
        private readonly SpriteFont font;

        public ScoreWidget(SpriteFont font)
        {
            this.font = font;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, "Score: " + Score, position, Color.White);
        }
    }
}
