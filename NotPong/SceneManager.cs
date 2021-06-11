using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace NotPong
{
    static class SceneManager
    {
        public static IScene CurrentScene { get; private set; }

        private static List<IScene> scenes = new List<IScene>();

        public static void AddScene(IScene scene)
        {
            scenes.Add(scene);
        }

        public static void LoadScene<T>()
        {
            var index = scenes.FindIndex(scene => scene is T);
            LoadScene(index);
        }

        public static void LoadScene(int index)
        {
            if (scenes.ElementAtOrDefault(index) is IScene scene)
            {
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
