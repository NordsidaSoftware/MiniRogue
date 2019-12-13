using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace MiniRogue
{
    internal class Actor
    {
        public int Char;
        public Color color;
        public int X, Y;
        public List<ESC> Components;

        public bool Alive { get; internal set; }
        public bool HasMoved { get; set; }

        public Actor(int Char, int X, int Y)
        {
            Components = new List<ESC>();
            this.Char = Char;
            this.X = X;
            this.Y = Y;
            Alive = true;
            HasMoved = true;  // First time in, the actor has not moved, but need to recalculate FOV
        }

        public void Update(GameTime gameTime)
        {
            foreach (ESC esc in Components) { if (!esc.Disabled )esc.Update(gameTime); }      
        }


        public void Draw(Display display)
        {
            display.SetChar(X, Y, Char, color);
        }

        internal void Draw(Display display, int x, int y)
        {
            display.SetChar(X-x, Y-y, Char, color);
        }
    }
}