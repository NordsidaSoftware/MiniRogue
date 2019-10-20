using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MiniRogue
{
    internal class Display
    {
        Texture2D font;
        Texture2D background;
        const int fontSize = 10;
        public Color BGColor = Color.Black;
        public Color FGColor = Color.White;
        internal int X;
        internal int Y;
        internal int Width;
        internal int Height;
        private int[,] Grid;
        private Color[,] BackgroundGrid;
        private Color[,] ForegroundGrid;
        private int CharSize;

        public Display(Texture2D font, Texture2D background, int X, int Y, int Width, int Height)
        {
            this.font = font;
            this.background = background;
            this.X = X;
            this.Y = Y;
            this.Width = Width;
            this.Height = Height;
            SetCharSize(10);
        }

        private void RecalculateGrid()
        {
            Grid = new int[Width / CharSize, Height / CharSize];
            BackgroundGrid = new Color [Width / CharSize, Height / CharSize];
            for ( int x = 0; x < BackgroundGrid.GetLength(0); x++)
            {
                for ( int y = 0; y < BackgroundGrid.GetLength(1); y++)
                {
                    BackgroundGrid[x, y] = FGColor;
                }
            }
            ForegroundGrid = new Color[Width / CharSize, Height / CharSize];
        }
        public void SetCharSize(int CharSize)
        {
            this.CharSize = CharSize;
            RecalculateGrid();
        }
        public void SetChar(int x, int y, int Char)
        {
            Grid[x, y] = Char;
            BackgroundGrid[x, y] = FGColor;
            ForegroundGrid[x, y] = BGColor;
        }

        public void PutChar(int x, int y, int Char)
        {
            Grid[x, y] = Char;
        }
        public void Write(string message)
        {
            int x = 0;
            int y = 0;
            for (int index = 0; index < message.Length; index++)
            {
                SetChar(x, y, message[index]);
                x++;
            }
        }
        internal void Fill(char Char)
        {
            for (int x = 0; x < Grid.GetLength(0); x++)
            {
                for (int y = 0; y < Grid.GetLength(1); y++)
                {
                    Grid[x, y] = Char;
                }
            }
        }

        internal void SetBackgroundColor(Color BGColor)
        {
            this.BGColor = BGColor;
        }
        internal void SetForegroundColor(Color FGColor)
        {
            this.FGColor = FGColor;
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
        public void Draw(SpriteBatch spriteBatch)
        {

            for (int x = 0; x < Grid.GetLength(0); x++)
            {
                for (int y = 0; y < Grid.GetLength(1); y++)
                {
                    spriteBatch.Draw(background, new Rectangle(x * CharSize + X, y * CharSize + Y, CharSize, CharSize)
                       , ForegroundGrid[x,y]);
                }
            }

                    for (int x = 0; x < Grid.GetLength(0); x++)
            {
                for (int y = 0; y < Grid.GetLength(1); y++)
                {
                    int x_source = Grid[x, y] % 16 * fontSize;
                    int y_source = Grid[x, y] / 16 * fontSize;
                    spriteBatch.Draw(font, new Rectangle(x * CharSize + X, y * CharSize + Y, CharSize, CharSize),
                        new Rectangle(x_source, y_source, 10, 10), BackgroundGrid[x,y]);
                }
            }
        }
    }
}