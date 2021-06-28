using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NotPong
{
    internal abstract class Bonus
    {
        public Texture2D Texture { get; set; }
        public bool Active { get; set; }

        protected Point index;
        private bool charged = true;

        public void Activate(Point index)
        {
            if (charged)
            {
                charged = false;
                this.index = index;
                Active = true;
            }
        }

        public abstract void Update(GameTime gameTime, GameGrid grid);
        public abstract void Draw(SpriteBatch spriteBatch, Vector2 position);
    }
}
