using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NotPong.Scenes;

namespace NotPong
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferHeight = GameSettings.height;
            graphics.PreferredBackBufferWidth = GameSettings.width;
            graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            var center = new Vector2(GameSettings.width / 2, GameSettings.height / 2);
            var playTexture = Content.Load<Texture2D>("buttons/play");
            var okTexture = Content.Load<Texture2D>("buttons/ok");
            var gameOverTexture = Content.Load<Texture2D>("gameover");
            var gameScene = new GameScene();
            gameScene.Load(Content);
            SceneManager.AddScene(new MainMenuScene(playTexture, center));
            SceneManager.AddScene(gameScene);
            SceneManager.AddScene(new EndScene(okTexture, gameOverTexture, center));
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            SceneManager.CurrentScene.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            SceneManager.CurrentScene.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
