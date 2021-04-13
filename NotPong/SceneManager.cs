using System;
using System.Collections.Generic;
using System.Text;

namespace NotPong
{
    static class SceneManager
    {
        public static IScene CurrentScene
        {
            get
            {
                return scenes[currentIndex];
            }
        }

        private static List<IScene> scenes = new List<IScene>();
        private static int currentIndex = 0;

        public static void AddScene(IScene scene)
        {
            scenes.Add(scene);
        }

        public static void LoadScene(int index)
        {
            if (scenes.Count > index) currentIndex = index;
        }
    }
}
