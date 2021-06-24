using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NotPong
{
    internal abstract class Bonus
    {
        public Texture2D Texture { get; set; }
        public bool Active { get; set; }

        protected Point index;
        private Block[,] grid;
        private bool charged = true;

        public void Activate(Block[,] grid, Point index)
        {
            if (charged)
            {
                charged = false;
                this.grid = grid;
                this.index = index;
                Active = true;
            }
        }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch, Vector2 position);

        protected static bool IsIndexInBounds(Point index)
        {
            const int size = GameSettings.GridSize;
            return index.X >= 0 && index.X < size && index.Y >= 0 && index.Y < size;
        }

        protected void FireBlock(Point doomIndex)
        {
            var doomBlock = grid[doomIndex.X, doomIndex.Y];
            doomBlock.ActivateBonus(grid);
            doomBlock.state = BlockState.Dead;
        }
    }
}
