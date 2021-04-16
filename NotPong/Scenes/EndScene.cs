using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NotPong.Scenes
{
    class EndScene : IScene
    {
        private UIElement button;
        private UIElement message;

        public EndScene(Texture2D buttonTexture, Texture2D messageTexture, Vector2 center)
        {
            var someSpace = new Vector2(0, center.Y / 4);
            message = new UIElement(messageTexture, center - someSpace);
            button = new UIElement(buttonTexture, center + someSpace);
            button.OnClick = ButtonOnClick;
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
            SceneManager.LoadScene(0);
        }
    }
}
