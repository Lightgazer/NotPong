using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NotPong
{
    internal interface IScene
    {
        public void Start() { }
        public void Stop() { }
        public void Update(GameTime gameTime);
        public void Draw(SpriteBatch spriteBatch);
    }
}
