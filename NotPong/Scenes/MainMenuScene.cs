using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace NotPong
{
    internal class MainMenuScene : IScene
    {
        private readonly UIElement button;

        public MainMenuScene(ContentManager content)
        {
            var center = new Vector2(GameSettings.Width / 2, GameSettings.Height / 2);
            var buttonTexture = content.Load<Texture2D>("buttons/play");
            button = new UIElement(buttonTexture, center) { onClick = ButtonOnClick };
        }
        public void Update(GameTime gameTime)
        {
            button.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            button.Draw(spriteBatch);
        }

        private void ButtonOnClick()
        {
            SceneManager.LoadScene<GameScene>();
        }
    }
}
