using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NotPong
{
    class ScoreView : IScore
    {
        private Vector2 position = new Vector2((GameSettings.width - GameSettings.gridSize * GameSettings.blockSize) / 2, 10);
        private int score = 0;
        private SpriteFont font;

        public ScoreView(SpriteFont font)
        {
            this.font = font;
        }

        public void Add(int score)
        {
            this.score += score;    
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, "Score " + score, position, Color.White);
        }
    }
}
