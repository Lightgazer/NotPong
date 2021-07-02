using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace NotPong
{
    internal static class SceneManager
    {
        public static IScene CurrentScene { get; private set; }

        private static readonly List<IScene> Scenes = new List<IScene>();

        public static void AddScene(IScene scene)
        {
            Scenes.Add(scene);
        }

        public static void LoadScene<T>()
        {
            var index = Scenes.FindIndex(scene => scene is T);
            LoadScene(index);
        }

        public static void LoadScene(int index)
        {
            if (Scenes.ElementAtOrDefault(index) is { } scene)
            {
                CurrentScene.Stop();
                CurrentScene = scene;
                CurrentScene.Start();
            }
        }

        public static void Update(GameTime gameTime)
        {
            CurrentScene.Update(gameTime);
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            CurrentScene.Draw(spriteBatch);
        }
    }
}
