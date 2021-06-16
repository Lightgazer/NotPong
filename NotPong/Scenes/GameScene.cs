using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace NotPong
{
    class GameScene : IScene
    {
        public static int numberOfBlockTypes = 5;

        private readonly Texture2D[] blockTextures;
        private readonly Texture2D frameTexture;
        private readonly Texture2D bombTexture;
        private readonly Texture2D lineTexture;
        private readonly SpriteFont font;

        private GameGrid grid;
        private ScoreWidget scoreView;
        private SceneTimer timer;

        public GameScene(ContentManager content)
        {
            blockTextures = LoadBlockTextures(content);
            frameTexture = content.Load<Texture2D>("frame");
            bombTexture = content.Load<Texture2D>("bonuses/bomb");
            lineTexture = content.Load<Texture2D>("bonuses/line");
            font = content.Load<SpriteFont>("Font");
        }

        public void Start()
        {
            grid = new GameGrid(blockTextures, frameTexture, lineTexture, bombTexture);
            scoreView = new ScoreWidget(font);
            grid.Score = scoreView;
            timer = new SceneTimer(font, 60d);
        }

        public void Update(GameTime gameTime)
        {
            grid.Update(gameTime);
            timer.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            grid.Draw(spriteBatch);
            scoreView.Draw(spriteBatch);
            timer.Draw(spriteBatch);
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
