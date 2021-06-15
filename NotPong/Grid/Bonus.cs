using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NotPong
{
    abstract class Bonus
    {
        public Texture2D BonusTexture { get; set; }
        public bool Active { get; set; } = false;

        protected Block[,] grid;
        protected Point index;
        protected bool charged = true;

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

        protected bool IsIndexInBounds(Point index)
        {
            var length1 = grid.GetLength(0);
            var length2 = grid.GetLength(1);
            if (index.X >= 0 && index.X < length1 && index.Y >= 0 && index.Y < length2)
                return true;
            return false;
        }

        protected void FireBlock(Point doomIndex)
        {
            var doomBlock = grid[doomIndex.X, doomIndex.Y];
            doomBlock.FireBonus(grid);
            doomBlock.state = BlockState.Dead;
        }
    }
}
