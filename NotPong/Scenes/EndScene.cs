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
            var buttonOrigin = new Vector2(buttonTexture.Width / 2, buttonTexture.Height / 2);
            var buttonVector = center - someSpace + buttonOrigin;
            buttonRectangle = new Rectangle(buttonVector.ToPoint(), new Point(buttonTexture.Width, buttonTexture.Height));
            var messageOrigin = new Vector2(messageTexture.Width / 2, messageTexture.Height / 2);
            var messageVector = center + someSpace + messageOrigin;
            messageRectangle = new Rectangle(messageVector.ToPoint(), new Point(messageTexture.Width, messageTexture.Height));

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
