using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace MiniRogue
{
    internal class Item
    {
        public int Char;
        public Color Color;
        public int X, Y;
        public List<ESC> Components;

        public Item(int Char, int x, int y)
        {
            this.Char = Char;
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
            display.SetChar(X, Y, Char, Color);
        }
    }

   

    internal class DStair : Item
    {
        public DStair(int x, int y) : base('>', x, y) { Color = Color.Yellow; }
    }
}