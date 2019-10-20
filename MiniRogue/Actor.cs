using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace MiniRogue
{
    internal class Actor
    {
        public int Char;
        public int X, Y;
        public List<ESC> Components;

        public Actor(int Char, int X, int Y)
        {
            Components = new List<ESC>();
            this.Char = Char;
            this.X = X;
            this.Y = Y;
        }

        public void Update(GameTime gameTime)
        {
            foreach (ESC esc in Components) { esc.Update(gameTime); }
        }


        public void Draw(Display display)
        {
            display.PutChar(X, Y, Char);
        }
    }
}