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
    class StartScene : IScene
    {
        private Texture2D buttonTexture;
        private Rectangle buttonRectangle;
        private MouseState lastMouseState;

        public StartScene(Texture2D buttonTexture,  Vector2 center)
        {
            this.buttonTexture = buttonTexture;

            var someSpace = new Vector2(0, center.Y / 4);
            var buttonOrigin = new Vector2(buttonTexture.Width / 2, buttonTexture.Height / 2);
            var buttonVector = center - someSpace + buttonOrigin;
            buttonRectangle = new Rectangle(buttonVector.ToPoint(), new Point(buttonTexture.Width, buttonTexture.Height));

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
