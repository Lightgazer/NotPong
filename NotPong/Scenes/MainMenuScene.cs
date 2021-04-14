using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace NotPong.Scenes
{
    //сделать родителя
    //добавить имя
    class MainMenuScene : IScene
    {
        private Texture2D buttonTexture;
        private Rectangle buttonRectangle;
        private MouseState lastMouseState;
        private Vector2 center;

        public MainMenuScene(Texture2D buttonTexture,  Vector2 center)
        {
            this.buttonTexture = buttonTexture;
            this.center = center;

            var buttonSize = new Vector2(buttonTexture.Width, buttonTexture.Height);
            var buttonVector = center - buttonSize / 2;
            buttonRectangle = new Rectangle(buttonVector.ToPoint(), buttonSize.ToPoint());

            lastMouseState = Mouse.GetState();
        }

        public void Draw(GameTime gameTime, GraphicsDeviceManager graphics, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(buttonTexture, buttonRectangle, Color.White);
        }

        public void Update(GameTime gameTime)
        {
            if (IsButtonPressed()) ButtonOnClick();
        }

        private bool IsButtonPressed()
        {
            var mouseState = Mouse.GetState();
            if (lastMouseState.LeftButton == ButtonState.Released && mouseState.LeftButton == ButtonState.Pressed)
            {
                if (buttonRectangle.Contains(mouseState.Position))
                {
                    return true;
                }
            }

            lastMouseState = mouseState;
            return false;
        }

        public void ButtonOnClick()
        {
            SceneManager.LoadScene(1);
        }
    }
}
