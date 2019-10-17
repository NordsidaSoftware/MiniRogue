using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MiniRogue
{
    internal class Display
    {
        Texture2D font;
        const int fontSize = 10;
        private int X;
        private int Y;
        private int Width;
        private int Height;
        private int[,] Grid;
        public int CharSize { get; set; } = 10;

        public Display(Texture2D font, int X, int Y, int Width, int Height)
        {
            this.font = font;
            this.X = X;
            this.Y = Y;
            this.Width = Width;
            this.Height = Height;
            Grid = new int[Width / CharSize, Height / CharSize];
           
        }

        public void PutChar(int x, int y, int Char)
        {
            Grid[x, y] = Char;
        }



        public void Draw(SpriteBatch spriteBatch)
        {
            for (int x = 0; x < Grid.GetLength(0); x++)
            {
                for ( int y = 0; y < Grid.GetLength(1); y++)
                {
                    int x_source = Grid[x, y] % 16 * fontSize;
                    int y_source = Grid[x, y] / 16 * fontSize;
                    spriteBatch.Draw(font, new Rectangle(x*CharSize, y*CharSize, CharSize, CharSize), 
                        new Rectangle(x_source, y_source, 10, 10), Color.White);
                }
            }
        }

        public void Clear()
        {
            for (int x = 0; x < Grid.GetLength(0); x++)
            {
                for (int y = 0; y < Grid.GetLength(1); y++)
                {
                    Grid[x, y] = 0;
                }
            }
        }
    }
}