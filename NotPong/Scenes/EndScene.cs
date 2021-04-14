using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace NotPong.Scenes
{
    class EndScene : IScene
    {
        private Texture2D buttonTexture;
        private Rectangle buttonRectangle;
        private Texture2D messageTexture;
        private Rectangle messageRectangle;

        private MouseState lastMouseState;

        public EndScene(Texture2D buttonTexture, Texture2D messageTexture, Vector2 center)
        {
            this.buttonTexture = buttonTexture;
            this.messageTexture = messageTexture;

            var someSpace = new Vector2(0, center.Y / 4);
            var buttonSize = new Vector2(buttonTexture.Width, buttonTexture.Height);
            var buttonVector = center - someSpace - buttonSize / 2;
            buttonRectangle = new Rectangle(buttonVector.ToPoint(), buttonSize.ToPoint());
            var messageSize = new Vector2(messageTexture.Width, messageTexture.Height);
            var messageVector = center + someSpace - messageSize / 2;
            messageRectangle = new Rectangle(messageVector.ToPoint(), messageSize.ToPoint());

            lastMouseState = Mouse.GetState();
        }

        public void Draw(GameTime gameTime, GraphicsDeviceManager graphics, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(buttonTexture, buttonRectangle, Color.White);
            spriteBatch.Draw(messageTexture, messageRectangle, Color.White);
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
            SceneManager.LoadScene(0);
        }
    }
}
