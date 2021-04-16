using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NotPong
{
    interface IScene
    {
        public void Start() { }
        public void Update(GameTime gameTime);
        public void Draw(SpriteBatch spriteBatch);
    }
}
