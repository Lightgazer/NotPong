using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using NotPong.Scenes;

namespace NotPong
{
    class GameGrid
    {
        const int gridSize = 8;
        const int blockSize = 64;

        private static readonly Random random = new Random();
        private static readonly object randomLock = new object();

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
                new Point((WindowSize.width - gridSize * blockSize) / 2, (WindowSize.height - gridSize * blockSize) / 2),
                new Point(gridSize * blockSize, gridSize * blockSize)
            );
        }

        public void Update(GameTime gameTime)
        {
            var option = GetCellClick();
            if (option is Click click)
            {
                if (selectedIndex is null)
                {
                    selectedIndex = click.Index;
                }
                else
                {
                    selectedIndex = null;
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
                    position += block.getPostion();
                    position += padding;
                    spriteBatch.Draw(blockTextures[block.type], position, Color.White);
                }

            }
        }

        private OptionClick GetCellClick()
        {
            var mouseState = Mouse.GetState();
            OptionClick option;
            if (lastMouseState.LeftButton == ButtonState.Released &&
                mouseState.LeftButton == ButtonState.Pressed &&
                gridRectangle.Contains(mouseState.Position))
            {
                var positionOnGrid = mouseState.Position - gridRectangle.Location;
                option = new Click {
                    Index = new Tuple<int, int>(positionOnGrid.X / blockSize, positionOnGrid.Y / blockSize) 
                };
            }
            else
            {
                option = new NoClick();
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
            lock (randomLock)
            {
                var type = random.Next(GameScene.numberOfBlockTypes);
                Block block = new Block(type);
                return block;
            }

        }
    }

    //😢😢😢 хочу Option<T> из языка rust 
    abstract class OptionClick { }

    class Click : OptionClick
    {
        public Tuple<int, int> Index { get; set; }
    }

    class NoClick : OptionClick { }
}
