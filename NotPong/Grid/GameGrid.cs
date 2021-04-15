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
        public IScore Score { private get; set; }

        private static int gridSize = GameSettings.gridSize;
        private static int blockSize = GameSettings.blockSize;
        private static readonly Random random = new Random();

        private Texture2D[] blockTextures;
        private Texture2D frameTexture;
        private Texture2D lineTexture;
        private Texture2D bombTexture;
        private Block[,] grid = new Block[gridSize, gridSize];
        private MouseState lastMouseState;
        private Rectangle gridRectangle;
        private Tuple<int, int> selectedIndex;

        public GameGrid(Texture2D[] blockTextures, Texture2D frameTexture, Texture2D lineTexture, Texture2D bombTexture)
        {
            this.blockTextures = blockTextures;
            this.frameTexture = frameTexture;
            this.lineTexture = lineTexture;
            this.bombTexture = bombTexture;
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
                Score.Add(CountDead());
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
            DrawBlocks(spriteBatch, padding);
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

        private void DrawBlocks(SpriteBatch spriteBatch, Vector2 padding)
        {
            for (int indexX = 0; indexX < gridSize; indexX++)
            {
                for (int indexY = 0; indexY < gridSize; indexY++)
                {
                    var block = grid[indexX, indexY];
                    var position = new Vector2((indexX * blockSize), (indexY * blockSize));
                    position += padding;
                    block.Draw(spriteBatch, position);
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
                        KillBlocks(matchChain, vertical);
                        currentType = block.type;
                        matchChain.Clear();
                        matchChain.Add(block);
                    }
                }
                KillBlocks(matchChain, vertical);
            }
        }

        private void KillBlocks(List<Block> matchChain, bool vertical)
        {
            if (matchChain.Count < 3) return;
            matchChain.ForEach(block =>
            {
                block.Fire();
                if (block.state == BlockState.Suspect && matchChain.Count > 3)
                {
                    if (matchChain.Count > 4)
                        block.Bonus = new Bonus { BonusTexture = bombTexture };
                    else if (matchChain.Count > 3)
                        block.Bonus = new Bonus { BonusTexture = lineTexture };
                    block.state = BlockState.Idle;
                }
                else
                {
                    //здесь запуск бонуса
                    block.state = BlockState.Dead;
                }
            });
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
                grid[x, y].MoveFrom(new Vector2(0, -1));
                y--;
            }

            grid[x, 0] = CreateBlock();
            grid[x, 0].MoveFrom(new Vector2(0, -1));
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
            var firstIndex = grid.Cast<Block>().ToList().FindIndex(block => block.state == BlockState.Suspect);
            var secondIndex = grid.Cast<Block>().ToList().FindLastIndex(block => block.state == BlockState.Suspect);
            if (firstIndex == -1)
                return;
            else if (firstIndex == secondIndex)
            {
                grid[firstIndex / gridSize, firstIndex % gridSize].state = BlockState.Idle;
            }
            else
            {
                var first = new Tuple<int, int>(firstIndex / gridSize, firstIndex % gridSize);
                var second = new Tuple<int, int>(secondIndex / gridSize, secondIndex % gridSize);
                SwapBlocks(first, second, BlockState.Idle);
            }
        }

        private int CountDead()
        {
            return grid.Cast<Block>().Where(block => block.state == BlockState.Dead).Count();
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

        private Block CreateBlock()
        {
            var type = random.Next(GameScene.numberOfBlockTypes);
            var block = new Block(type, blockTextures[type]);
            return block;
        }
    }
}
