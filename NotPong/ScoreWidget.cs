using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NotPong
{
    internal class ScoreWidget
    {
        private readonly Vector2 position = new Vector2((GameSettings.Width - GameSettings.GridSize * GameSettings.BlockSize) / 2, 10);
        private readonly SpriteFont font;
        private int score = 0;

        public ScoreWidget(SpriteFont font)
        {
            this.font = font;
        }

        public void Add(int score)
        {
            this.score += score;    
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, "Score: " + score, position, Color.White);
        }
    }
}
