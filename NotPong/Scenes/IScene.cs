using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NotPong
{
    interface IScene
    {
        public void Update(GameTime gameTime);
        public void Draw(GameTime gameTime, GraphicsDeviceManager graphics, SpriteBatch spriteBatch);
    }
}
