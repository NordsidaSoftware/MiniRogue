using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace MiniRogue
{
    internal class Item
    {
        public int Char;
        public int X, Y;
        public bool Block;
        public List<ESC> Components;

        public Item(int @char, int x, int y, bool Block)
        {
            Char = @char;
            X = x;
            Y = y;
            this.Block = Block;
            Components = new List<ESC>();
        }


        public void Update(GameTime gameTime)
        {
            foreach (ESC esc in Components) { esc.Update(gameTime); }
        }

        public void Draw(Display display)
        {
            display.SetChar(X, Y, Char);
        }
    }
}