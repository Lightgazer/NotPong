using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace NotPong.Scenes
{
    class GameScene : IScene
    {
        public static int numberOfBlockTypes = 5;

        private Texture2D[] blockTextures;
        private Texture2D frameTexture;
        private Texture2D bombTexture;
        private Texture2D lineTexture;
        private SpriteFont font;

        private GameGrid grid;
        private ScoreView scoreView;

        public void Load(ContentManager content)
        {
            blockTextures = LoadBlockTextures(content);
            frameTexture = content.Load<Texture2D>("frame");
            bombTexture = content.Load<Texture2D>("bonuses/bomb");
            lineTexture = content.Load<Texture2D>("bonuses/line");
            font = content.Load<SpriteFont>("Font");
        }

        public void Start()
        {
            scoreView = new ScoreView(font);
            grid = new GameGrid(blockTextures, frameTexture, lineTexture, bombTexture);
            grid.Score = scoreView;
            //todo
            //time restriction
            //score
        }

        public void Update(GameTime gameTime)
        {
            grid.Update(gameTime);
        }

        public void Draw(GameTime gameTime, GraphicsDeviceManager graphics, SpriteBatch spriteBatch)
        {
            grid.Draw(spriteBatch);
            scoreView.Draw(spriteBatch);
        }

        private static Texture2D[] LoadBlockTextures(ContentManager content)
        {
            var textures = new Texture2D[numberOfBlockTypes];
            for (int index = 0; index < numberOfBlockTypes; index++)
            {
                textures[index] = content.Load<Texture2D>("blocks/block" + index.ToString());
            }
            return textures;
        }
    }
}
