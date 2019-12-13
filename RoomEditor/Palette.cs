using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RoomEditor
{
    internal class Palette : Display
    {
        public Color CurrentForeground;
        public Color CurrentBackground;

        List<Color> colorswatch;

        public Palette(Texture2D font, int X, int Y, int Width, int Height, bool Border = false) 
            : base(font, X, Y, Width, Height, Border)
        {
            colorswatch = new List<Color>();
            setupColorswatch();
        
        }

        public void setupGrid()
        {
            for (int index = 0; index < colorswatch.Count; index++)
            {
                Point p = new Point(index % (TileWidth-2), index / (TileHeight-2));
              
                   SetChar(p.X+1, p.Y+1, 219, colorswatch[index]);
                
            }
        }

        private void setupColorswatch()
        {
            colorswatch.Add(Color.White);
            colorswatch.Add(Color.Black);
            colorswatch.Add(Color.Red);
            colorswatch.Add(Color.Blue);
            colorswatch.Add(Color.Brown);
            colorswatch.Add(Color.Green);
            colorswatch.Add(Color.Yellow);
            colorswatch.Add(Color.Orange);
            colorswatch.Add(Color.Gray);
            colorswatch.Add(Color.Cyan);
            colorswatch.Add(Color.DarkOliveGreen);
        }


        internal Color GetColor(int x, int y)
        {
            return ForegroundGrid[x, y];
        }

        internal void SetColor(int x, int y)
        {
            CurrentForeground = GetColor(x, y);
        }

    }
}
