using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace MiniRogue
{
    internal class Item
    {
        public int Char;
        public int X, Y;
        public List<ESC> Components;

        public Item(int @char, int x, int y)
        {
            Char = @char;
            X = x;
            Y = y;
            Components = new List<ESC>();
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