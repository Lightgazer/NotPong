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
        //private Texture2D ballTexture;

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
            // TODO: use this.Content to load your game content here
            //ballTexture = Content.Load<Texture2D>("ball");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            //var kstate = Keyboard.GetState();

            //if (kstate.IsKeyDown(Keys.Up))
            //    ballPosition.Y -= ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            //if (kstate.IsKeyDown(Keys.Down))
            //    ballPosition.Y += ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            //if (kstate.IsKeyDown(Keys.Left))
            //    ballPosition.X -= ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            //if (kstate.IsKeyDown(Keys.Right))
            //    ballPosition.X += ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            //var mouse = Mouse.GetState(Window);

            SceneManager.CurrentScene.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            //var center = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);
            //var ballOrigin = new Vector2(ballTexture.Width / 2, ballTexture.Height / 2);
            //_spriteBatch.Draw(ballTexture, center, null, Color.White, 0f, ballOrigin, Vector2.One, SpriteEffects.None, 0f);
            SceneManager.CurrentScene.Draw(gameTime, graphics, spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

//System.Diagnostics.Debug.WriteLine(indexX.ToString() + " " + indexY.ToString());
