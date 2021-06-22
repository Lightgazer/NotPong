using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NotPong
{
    internal class GameGrid
    {
        public ScoreWidget Score { private get; set; }

        private static int gridSize = GameSettings.GridSize;
        private static int blockSize = GameSettings.BlockSize;
        private static readonly Random Random = new Random();

        private readonly Texture2D[] blockTextures;
        private readonly Texture2D frameTexture;
        private readonly Texture2D lineTexture;
        private readonly Texture2D bombTexture;
        private readonly Block[,] grid = new Block[gridSize, gridSize];
        private MouseState lastMouseState;
        private Rectangle gridRectangle;
        private Point? selectedIndex;

        public GameGrid(Texture2D[] blockTextures, Texture2D frameTexture, Texture2D lineTexture, Texture2D bombTexture)
        {
            this.blockTextures = blockTextures;
            this.frameTexture = frameTexture;
            this.lineTexture = lineTexture;
            this.bombTexture = bombTexture;
            PopulateGrid();

            gridRectangle = new Rectangle(
                new Point((GameSettings.Width - gridSize * blockSize) / 2, (GameSettings.Height - gridSize * blockSize) / 2),
                new Point(gridSize * blockSize, gridSize * blockSize)
            );
        }

        public void Update(GameTime gameTime)
        {
            UpdateBlocks(gameTime);
            Score?.Add(CountNewDead());

            if (IsReadyForMatch())
            {
                TriggerMatches();
                ReturnBlocks();
            }
            if (IsReadyForDrop()) TriggerDrop();
            if (IsIdle()) ManagePlayerInput();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var padding = new Vector2(gridRectangle.Location.X, gridRectangle.Location.Y);
            DrawBlocks(spriteBatch, padding);
            DrawBonuses(spriteBatch, padding);
            DrawFrame(spriteBatch, padding);
        }

        private void DrawFrame(SpriteBatch spriteBatch, Vector2 padding)
        {
            if (selectedIndex is { } pointIndex)
            {
                var position = new Vector2(pointIndex.X * blockSize, pointIndex.Y * blockSize);
                position += padding;
                spriteBatch.Draw(frameTexture, position, Color.White);
            }
        }

        private void DrawBlocks(SpriteBatch spriteBatch, Vector2 padding)
        {
            for (var indexX = 0; indexX < gridSize; indexX++)
            {
                for (var indexY = 0; indexY < gridSize; indexY++)
                {
                    var block = grid[indexX, indexY];
                    var position = new Vector2((indexX * blockSize), (indexY * blockSize));
                    position += padding;
                    block.Draw(spriteBatch, position);
                }
            }
        }

        private void DrawBonuses(SpriteBatch spriteBatch, Vector2 padding)
        {
            for (var indexX = 0; indexX < gridSize; indexX++)
            {
                for (var indexY = 0; indexY < gridSize; indexY++)
                {
                    var block = grid[indexX, indexY];
                    var position = new Vector2((indexX * blockSize), (indexY * blockSize));
                    position += padding;
                    block.DrawBonus(spriteBatch, position);
                }
            }
        }

        private void TriggerMatches()
        {
            TriggerMatches(true);
            TriggerMatches(false);
            CleanCrossingFlags();
        }

        private void TriggerMatches(bool vertical)
        {
            for (var indexX = 0; indexX < gridSize; indexX++)
            {
                var currentType = -1;
                var matchChain = new List<Block>();
                for (var indexY = 0; indexY < gridSize; indexY++)
                {
                    var block = vertical ? grid[indexY, indexX] : grid[indexX, indexY];
                    if (currentType == block.type)
                    {
                        matchChain.Add(block);
                    }
                    else
                    {
                        ProcessChain(matchChain, vertical);
                        currentType = block.type;
                        matchChain.Clear();
                        matchChain.Add(block);
                    }
                }
                ProcessChain(matchChain, vertical);
            }
        }

        private void ProcessChain(List<Block> matchChain, bool vertical)
        {
            if (matchChain.Count < 3) return;
            matchChain.ForEach(block =>
            {
                block.FireBonus(grid);
                if (block.crossingFlag)
                {
                    Score.Add(1);
                    block.state = BlockState.Idle;
                    block.Bonus = new BombBonus { BonusTexture = bombTexture };
                }
                else if (block.state == BlockState.Suspect && matchChain.Count > 3)
                {
                    if (matchChain.Count > 4)
                        block.Bonus = new BombBonus { BonusTexture = bombTexture };
                    else
                        block.Bonus = new LineBonus { BonusTexture = lineTexture, vertical = vertical };
                    Score.Add(1);
                    block.state = BlockState.Idle;
                }
                else
                {
                    block.state = BlockState.Dead;
                }
                block.crossingFlag = true;
            });
        }

        private void CleanCrossingFlags()
        {
            grid.Cast<Block>().ToList().ForEach(block => block.crossingFlag = false);
        }

        private void TriggerDrop()
        {
            for (var indexY = gridSize - 1; indexY >= 0; indexY--)
            {
                for (var indexX = 0; indexX < gridSize; indexX++)
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

        private void UpdateBlocks(GameTime gameTime)
        {
            grid.Cast<Block>().ToList().ForEach(block => block.Update(gameTime));
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

        private bool IsSwapAllowed(Point first, Point second)
        {
            if (first.X == second.X)
                return Math.Abs(first.Y - second.Y) == 1;
            if (first.Y == second.Y)
                return Math.Abs(first.X - second.X) == 1;
            return false;
        }

        private void SwapBlocks(Point first, Point second, BlockState setState)
        {
            var movementDirection = (first - second).ToVector2();
            var firstBlock = grid[first.X, first.Y];
            var secondBlock = grid[second.X, second.Y];
            firstBlock.state = setState;
            secondBlock.state = setState;
            firstBlock.MoveFrom(movementDirection);
            secondBlock.MoveFrom(-movementDirection);
            grid[second.X, second.Y] = firstBlock;
            grid[first.X, first.Y] = secondBlock;
        }

        private void ReturnBlocks()
        {
            var firstIndex = grid.Cast<Block>().ToList().FindIndex(block => block.state == BlockState.Suspect);
            var secondIndex = grid.Cast<Block>().ToList().FindLastIndex(block => block.state == BlockState.Suspect);
            if (firstIndex == -1) 
                return;
            
            if (firstIndex == secondIndex)
            {
                grid[firstIndex / gridSize, firstIndex % gridSize].state = BlockState.Idle;
            }
            else
            {
                var first = new Point(firstIndex / gridSize, firstIndex % gridSize);
                var second = new Point(secondIndex / gridSize, secondIndex % gridSize);
                SwapBlocks(first, second, BlockState.Idle);
            }
        }

        private int CountNewDead()
        {
            var newArrivals = grid.Cast<Block>()
                .Where(block => block.state == BlockState.Dead)
                .Where(block => !block.morgueTiket)
                .ToList();
            var ret = newArrivals.Count();
            newArrivals.ForEach(block => block.morgueTiket = true);
            return ret;
        }

        private void ManagePlayerInput()
        {
            var option = GetCellClick();
            if (option is { } click)
            {
                if(selectedIndex is { } pointIndex)
                {
                    if (IsSwapAllowed(pointIndex, click))
                    {
                        SwapBlocks(pointIndex, click, BlockState.Suspect);
                        selectedIndex = null;
                    }
                    else
                    {
                        selectedIndex = click;
                    }
                }
                else
                {
                    selectedIndex = click;
                }
            }
        }

        private Point? GetCellClick()
        {
            var mouseState = Mouse.GetState();
            Point? option = null;
            if (lastMouseState.LeftButton == ButtonState.Released &&
                mouseState.LeftButton == ButtonState.Pressed &&
                gridRectangle.Contains(mouseState.Position))
            {
                var (x, y) = mouseState.Position - gridRectangle.Location;
                option = new Point(x / blockSize, y / blockSize);
            }

            lastMouseState = mouseState;
            return option;
        }

        private void PopulateGrid()
        {
            for (var indexX = 0; indexX < gridSize; indexX++)
                for (var indexY = 0; indexY < gridSize; indexY++)
                    grid[indexX, indexY] = CreateBlock();
        }

        private Block CreateBlock()
        {
            var type = Random.Next(GameScene.NumberOfBlockTypes);
            var block = new Block(type, blockTextures[type]);
            return block;
        }
    }
}
