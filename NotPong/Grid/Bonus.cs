using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NotPong
{
    abstract class Bonus
    {
        public Texture2D BonusTexture { get; set; }
        public bool Active { get; set; } = false;

        protected Block[,] grid;
        protected (int, int) index;
        protected bool charged = true;

        public void Activate(Block[,] grid, (int, int) index)
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

        protected bool IsIndexInBounds((int, int) index)
        {
            var length1 = grid.GetLength(0);
            var length2 = grid.GetLength(1);
            if (index.Item1 > 0 && index.Item1 < length1 && index.Item2 > 0 && index.Item2 < length2)
                return true;
            return false;
        }

        protected void FireBlock((int, int) doomIndex)
        {
            var doomBlock = grid[doomIndex.Item1, doomIndex.Item2];
            doomBlock.FireBonus(grid);
            doomBlock.state = BlockState.Dead;
        }
    }
}
