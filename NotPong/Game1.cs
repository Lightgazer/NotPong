﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NotPong
{
    public class Game1 : Game
    {
        private Texture2D ballTexture;
        private Vector2 ballPosition;
        float ballSpeed;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            var center = new Vector2(_graphics.PreferredBackBufferHeight / 2, _graphics.PreferredBackBufferWidth / 2);
            ballPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);
            ballSpeed = 100f;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            ballTexture = Content.Load<Texture2D>("ball");
            //ballTexture = Content.Load<Texture2D>("blocks/block0");
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
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            //var ballOrigin = new Vector2(ballTexture.Width / 2, ballTexture.Height / 2);
            //_spriteBatch.Draw(ballTexture, ballPosition, null, Color.White, 0f, ballOrigin, Vector2.One, SpriteEffects.None, 0f);
            SceneManager.CurrentScene.Draw(gameTime, _graphics, _spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
