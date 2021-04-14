using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NotPong
{
    class Block
    {
        public int type;

        public Block(int type)
        {
            this.type = type;
        }

        public void Update(GameTime gameTime)
        {

        }

        /// <summary>
        ///    Накладывает движение анимации
        /// </summary>
        public Vector2 getPostion()
        {
            return new Vector2(0, 0);
        }
    }
}
