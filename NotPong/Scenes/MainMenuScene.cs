using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NotPong.Scenes
{
    //сделать родителя
    //добавить имя
    class MainMenuScene : IScene
    {
        private UIElement button;

        public MainMenuScene(Texture2D buttonTexture, Vector2 center)
        {
            button = new UIElement(buttonTexture, center);
            button.OnClick = ButtonOnClick;
        }
        public void Update(GameTime gameTime)
        {
            button.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            button.Draw(spriteBatch);
        }

        public void ButtonOnClick()
        {
            SceneManager.LoadScene(1);
        }
    }
}
