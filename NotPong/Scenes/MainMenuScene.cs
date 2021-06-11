using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace NotPong
{
    //добавить имя
    class MainMenuScene : IScene
    {
        private UIElement button;

        public MainMenuScene(ContentManager content)
        {
            var center = new Vector2(GameSettings.width / 2, GameSettings.height / 2);
            var buttonTexture = content.Load<Texture2D>("buttons/play");
            button = new UIElement(buttonTexture, center) { OnClick = ButtonOnClick };
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
            SceneManager.LoadScene<GameScene>();
        }
    }
}
