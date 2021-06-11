using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace NotPong
{
    class EndScene : IScene
    {
        private UIElement button;
        private UIElement message;

        public EndScene(ContentManager content)
        {
            var center = new Vector2(GameSettings.width / 2, GameSettings.height / 2);
            var buttonTexture = content.Load<Texture2D>("buttons/ok");
            var messageTexture = content.Load<Texture2D>("gameover");
            var someSpace = new Vector2(0, center.Y / 4);
            message = new UIElement(messageTexture, center - someSpace);
            button = new UIElement(buttonTexture, center + someSpace) { OnClick = ButtonOnClick };
        }

        public void Update(GameTime gameTime)
        {
            message.Update(gameTime);
            button.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            message.Draw(spriteBatch);
            button.Draw(spriteBatch);
        }

        public void ButtonOnClick()
        {
            SceneManager.LoadScene<MainMenuScene>();
        }
    }
}
