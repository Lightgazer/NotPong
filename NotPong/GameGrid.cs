using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using NotPong.Scenes;

namespace NotPong
{
    class GameGrid
    {
        private static int gridSize = GameSettings.gridSize;
        private static int blockSize = GameSettings.blockSize;
        private static Vector2 origin = new Vector2(blockSize / 2);
        private static readonly Random random = new Random();

        private Texture2D[] blockTextures;
        private Texture2D frameTexture;
        private Block[,] grid = new Block[gridSize, gridSize];
        private MouseState lastMouseState;
        private Rectangle gridRectangle;
        private Tuple<int, int> selectedIndex;

        public GameGrid(Texture2D[] blockTextures, Texture2D frameTexture)
        {
            this.blockTextures = blockTextures;
            this.frameTexture = frameTexture;
            PopulateGrid();

            gridRectangle = new Rectangle(
                new Point((GameSettings.width - gridSize * blockSize) / 2, (GameSettings.height - gridSize * blockSize) / 2),
                new Point(gridSize * blockSize, gridSize * blockSize)
            );
        }

        public void Update(GameTime gameTime)
        {
            grid.Cast<Block>().ToList().ForEach(block => block.Update(gameTime));

            if (IsReadyForMatch())
            {
                TriggerMatches();
                ReturnBlocks();
            }
            if (IsReadyForDrop()) TriggerDrop();
            if (IsIdle())
            {
                var option = GetCellClick();
                if (option is Tuple<int, int> click)
                {
                    if (selectedIndex == null)
                    {
                        selectedIndex = click;
                    }
                    else
                    {
                        if (IsSwapAllowed(selectedIndex, click))
                        {
                            SwapBlocks(selectedIndex, click, BlockState.Suspect);
                            selectedIndex = null;
                        }
                        else
                        {
                            selectedIndex = click;
                        }
                    }
                }

            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var padding = new Vector2(gridRectangle.Location.X, gridRectangle.Location.Y);
            DrawCells(spriteBatch, padding);
            DrawFrame(spriteBatch, padding);
        }

        private void DrawFrame(SpriteBatch spriteBatch, Vector2 padding)
        {
            if (selectedIndex != null)
            {
                var position = new Vector2(selectedIndex.Item1 * blockSize, selectedIndex.Item2 * blockSize);
                position += padding;
                spriteBatch.Draw(frameTexture, position, Color.White);
            }
        }

        private void DrawCells(SpriteBatch spriteBatch, Vector2 padding)
        {
            for (int indexX = 0; indexX < gridSize; indexX++)
            {
                for (int indexY = 0; indexY < gridSize; indexY++)
                {
                    var block = grid[indexX, indexY];
                    var position = new Vector2((indexX * blockSize), (indexY * blockSize));
                    position += block.MovementDisplacement;
                    position += padding;
                    position += origin;
                    spriteBatch.Draw(
                        blockTextures[block.type],
                        position,
                        null,
                        Color.White,
                        0f,
                        origin,
                        block.Size,
                        SpriteEffects.None,
                        0f
                        );
                }

            }
        }

        private void TriggerMatches()
        {
            TriggerMatches(true);
            TriggerMatches(false);
        }

        private void TriggerMatches(bool vertical)
        {
            for (int indexX = 0; indexX < gridSize; indexX++)
            {
                int currentType = -1;
                List<Block> matchChain = new List<Block>();
                for (int indexY = 0; indexY < gridSize; indexY++)
                {
                    var block = vertical ? grid[indexX, indexY] : grid[indexY, indexX];
                    if (currentType == block.type)
                    {
                        matchChain.Add(block);
                    }
                    else
                    {
                        KillBlocks(matchChain);
                        currentType = block.type;
                        matchChain.Clear();
                        matchChain.Add(block);
                    }
                }
                KillBlocks(matchChain);
            }
        }

        private void KillBlocks(List<Block> matchChain)
        {
            if (matchChain.Count > 2)
            {
                matchChain.ForEach(block =>
                {
                    //здесь проверка на суспект и коунт
                    //здесь запуск бонуса
                    block.Fire();
                    block.state = BlockState.Dead;
                });
            }
        }



        private void TriggerDrop()
        {
            for (int indexY = gridSize - 1; indexY >= 0; indexY--)
            {
                for (int indexX = 0; indexX < gridSize; indexX++)
                {
                    var block = grid[indexX, indexY];
                    if (block.state == BlockState.Rotten)
                        MarkDrop(indexX, indexY);
                }
            }
        }

        private void MarkDrop(int x, int y)
        {
            while (y > 0)
            {
                grid[x, y] = grid[x, y - 1];
                grid[x, y].MoveFrom(Direction.Up);
                y--;
            }

            grid[x, 0] = CreateBlock();
            grid[x, 0].MoveFrom(Direction.Up);
        }

        private bool IsReadyForMatch()
        {
            return grid.Cast<Block>().All(block => block.state == BlockState.Idle || block.state == BlockState.Suspect);
        }

        private bool IsIdle()
        {
            return grid.Cast<Block>().All(block => block.state == BlockState.Idle);
        }

        private bool IsReadyForDrop()
        {
            return grid.Cast<Block>().Any(block => block.state == BlockState.Rotten)
                && grid.Cast<Block>().All(block => block.state != BlockState.Moving);
        }

        private bool IsSwapAllowed(Tuple<int, int> first, Tuple<int, int> second)
        {
            if (first.Item1 == second.Item1)
                return Math.Abs(first.Item2 - second.Item2) == 1;
            else if (first.Item2 == second.Item2)
                return Math.Abs(first.Item1 - second.Item1) == 1;
            return false;
        }

        private void SwapBlocks(Tuple<int, int> first, Tuple<int, int> second, BlockState setState)
        {
            var movementDirection = new Vector2(first.Item1 - second.Item1, first.Item2 - second.Item2);
            var firstBlock = grid[first.Item1, first.Item2];
            var secondBlock = grid[second.Item1, second.Item2];
            firstBlock.state = setState;
            secondBlock.state = setState;
            firstBlock.MoveFrom(movementDirection);
            secondBlock.MoveFrom(-movementDirection);
            grid[second.Item1, second.Item2] = firstBlock;
            grid[first.Item1, first.Item2] = secondBlock;
        }

        private void ReturnBlocks()
        {
            //welp не нужно было 2d массив использовать
            var index = grid.Cast<Block>().ToList().FindIndex(block => block.state == BlockState.Suspect);
            if (index == -1) return;
            var first = new Tuple<int, int>(index / gridSize, index % gridSize);

            Tuple<int, int> second;
            if (grid[first.Item1 + 1, first.Item2].state == BlockState.Suspect)
            {
                second = new Tuple<int, int>(first.Item1 + 1, first.Item2);
            }
            else if (grid[first.Item1 - 1, first.Item2].state == BlockState.Suspect)
            {
                second = new Tuple<int, int>(first.Item1 - 1, first.Item2);
            }
            else if (grid[first.Item1, first.Item2 + 1].state == BlockState.Suspect)
            {
                second = new Tuple<int, int>(first.Item1, first.Item2 + 1);
            }
            else if (grid[first.Item1, first.Item2 - 1].state == BlockState.Suspect)
            {
                second = new Tuple<int, int>(first.Item1, first.Item2 - 1);
            }
            else
            {
                return;
            }

            SwapBlocks(first, second, BlockState.Idle);
        }

        private Tuple<int, int> GetCellClick()
        {
            var mouseState = Mouse.GetState();
            Tuple<int, int> option = null; //😢😢😢 хочу Option<T> из языка rust 
            if (lastMouseState.LeftButton == ButtonState.Released &&
                mouseState.LeftButton == ButtonState.Pressed &&
                gridRectangle.Contains(mouseState.Position))
            {
                var positionOnGrid = mouseState.Position - gridRectangle.Location;
                option = new Tuple<int, int>(positionOnGrid.X / blockSize, positionOnGrid.Y / blockSize);
            }

            lastMouseState = mouseState;
            return option;
        }

        private void PopulateGrid()
        {
            for (int indexX = 0; indexX < gridSize; indexX++)
                for (int indexY = 0; indexY < gridSize; indexY++)
                    grid[indexX, indexY] = CreateBlock();
        }

        private static Block CreateBlock()
        {
            var type = random.Next(GameScene.numberOfBlockTypes);
            Block block = new Block(type);
            return block;
        }
    }
}
