using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace NotPong
{
    class UIElement
    {
        public Action OnClick;

        protected Texture2D texture;
        protected Rectangle rectangle;

        private MouseState lastMouseState;

        public UIElement(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            var size = new Vector2(texture.Width, texture.Height);
            var vector = position - size / 2;
            this.rectangle = new Rectangle(vector.ToPoint(), size.ToPoint());
            lastMouseState = Mouse.GetState();
        }

        public void Update(GameTime gameTime)
        {
            if (CheckClick()) this?.OnClick();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rectangle, Color.White);
        }

        private bool CheckClick()
        {
            var mouseState = Mouse.GetState();
            if (lastMouseState.LeftButton == ButtonState.Released && mouseState.LeftButton == ButtonState.Pressed)
            {
                if (rectangle.Contains(mouseState.Position))
                {
                    return true;
                }
            }

            lastMouseState = mouseState;
            return false;
        }
    }
}
