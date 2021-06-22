using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NotPong
{
    internal class SceneTimer
    {
        private readonly Vector2 position = new Vector2(GameSettings.Width * 0.6f, 10);
        private readonly SpriteFont font;
        private double timeLeft;

        public SceneTimer(SpriteFont font, double seconds)
        {
            this.font = font;
            timeLeft = seconds;
        }

        public void Update(GameTime gameTime)
        {
            timeLeft -= gameTime.ElapsedGameTime.TotalSeconds;
            if (timeLeft < 0) SceneManager.LoadScene<EndScene>();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, "Time Left: " + (int)timeLeft, position, Color.White);
        }
    }
}
