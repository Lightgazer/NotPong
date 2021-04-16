using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NotPong
{
    class SceneTimer
    {
        private Vector2 position = new Vector2(GameSettings.width * 0.6f, 10);
        private SpriteFont font;
        private double timeLeft;

        public SceneTimer(SpriteFont font, double seconds)
        {
            this.font = font;
            timeLeft = seconds;
        }

        public void Update(GameTime gameTime)
        {
            timeLeft -= gameTime.ElapsedGameTime.TotalSeconds;
            if (timeLeft < 0) SceneManager.LoadScene(2);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, "Time Left: " + (int)timeLeft, position, Color.White);
        }
    }
}
