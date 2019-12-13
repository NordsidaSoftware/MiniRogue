using System;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RoomEditor
{
    internal class Display
    {
        Texture2D font;
        const int fontSize = 10;
        public Color BGColor = Color.Transparent;
        public Color FGColor = Color.White;
        
        internal int X;
        internal int Y;
        internal int Width;
        internal int Height;
        internal int[,] Grid;
        internal Color[,] BackgroundGrid;
        internal Color[,] ForegroundGrid;
        public int CharSize;

        private Point Cursor;

       
        public int CurrentChar;
        public Rectangle Rectangle;
        
      
        public bool Border { get; private set; }

        public int TileWidth { get { return Grid.GetLength(0)-2; } }
        public int TileHeight { get { return Grid.GetLength(1)-2; } }

        public Display(Texture2D font, int X, int Y, int Width, int Height, bool Border = false)
        {
            this.font = font;
            this.X = X;
            this.Y = Y;
            this.Width = Width;
            this.Height = Height;
            Cursor = new Point(0, 0);
            SetCharSize(10);
            if (Border) { SetBorder(); }
            if (Border) { Rectangle = new Rectangle(X + CharSize, Y + CharSize, Width - CharSize*2, Height - CharSize*2); }
            else Rectangle = new Rectangle(X, Y, Width, Height);
        }

        public void Rescale(int dx, int dy)
        {
            if ((Width + dx * CharSize) > CharSize && ( Width + dx * CharSize <= 500 ))
                Width = Width + dx * CharSize;
            if ((Height + dy * CharSize) > CharSize && (Height + dy * CharSize <= 400))
                Height = Height + dy*CharSize;

            RecalculateGrid();
        }

        private void RecalculateGrid()
        {
            Grid = new int[Width / CharSize, Height / CharSize];
            BackgroundGrid = new Color [Width / CharSize, Height / CharSize];

            // .
            for ( int x = 0; x < BackgroundGrid.GetLength(0); x++)
            {
                for ( int y = 0; y < BackgroundGrid.GetLength(1); y++)
                {
                    BackgroundGrid[x, y] = BGColor;
                }
            }
            ForegroundGrid = new Color[Width / CharSize, Height / CharSize];
            for (int x = 0; x < ForegroundGrid.GetLength(0); x++)
            {
                for (int y = 0; y < ForegroundGrid.GetLength(1); y++)
                {
                    ForegroundGrid[x, y] = FGColor;
                }
            }
            if (Border) { SetBorder(); }
            if (Border) { Rectangle = new Rectangle(X + CharSize, Y + CharSize, Width - CharSize * 2, Height - CharSize * 2); }
           
        }

        internal int GetChar(int x, int y)
        {
            if ( x >= 0 && x < Grid.GetLength(0))
            {
                if ( y >= 0 && x < Grid.GetLength(1))
                {
                    return Grid[x, y];
                }
            }
            return 0;
        }
        public void SetCharSize(int CharSize)
        {
            this.CharSize = CharSize;
            RecalculateGrid();
        }
        public void SetChar(int x, int y, int Char)
        {
            Grid[x, y] = Char;
            BackgroundGrid[x, y] = BGColor;
            ForegroundGrid[x, y] = FGColor;
        }

        public void SetChar(int x, int y, int Char, Color color)
        {
            ForegroundGrid[x, y] = color;
            Grid[x, y] = Char;
        }

        public void SetChar(Point point, int Char, Color color)
        {
            SetChar(point.X, point.Y, Char, color);
        }
        private void SetBorder()
        {
            Border = true;
            Cursor = new Point(1, 1);
            for ( int x = 1; x < Grid.GetLength(0)-1; x++)
            {
                SetChar(x, 0, 196);
                SetChar(x, Grid.GetLength(1)-1, 196);
            }
            for ( int y = 1; y < Grid.GetLength(1)-1; y++)
            {
                SetChar(0, y, 179);
                SetChar(Grid.GetLength(0) - 1, y, 179);
            }
            SetChar(0, 0, 218);
            SetChar(Grid.GetLength(0)-1, 0, 191);
            SetChar(0, Grid.GetLength(1)-1, 192);
            SetChar(Grid.GetLength(0)-1, Grid.GetLength(1)-1, 217);
        }
     
        public void Write(string message)
        { 
            for (int index = 0; index < message.Length; index++)
            {
                SetChar(Cursor.X, Cursor.Y, message[index]);
                AdvanceCursor();
            }
        }

        public void WriteLine(string message)
        {
            Write(message);
            LineShift();
        }
        private void AdvanceCursor()
        {
            Cursor.X++;
            if (Border) { if (Cursor.X >= Grid.GetLength(0) - 1) { LineShift(); } }
            else { if (Cursor.X >= Grid.GetLength(0)) { LineShift(); } }
        }

        private void LineShift()
        {
            Cursor.Y++;
            Cursor.X = Border ? 1 : 0;
            if (Border) { if (Cursor.Y >= Grid.GetLength(1) - 1) { Cursor = Border ? new Point(1, 1):Point.Zero; } }
            else { if (Cursor.Y >= Grid.GetLength(1)) { Cursor = Border ? new Point(1, 1) : Point.Zero; } }
        }

        internal void Fill(int Char) { Fill((char)Char); }
        internal void Fill(char Char)
        {
            int x_stop = Border ? Grid.GetLength(0) - 1 : Grid.GetLength(0);
            int y_stop = Border ? Grid.GetLength(1) - 1 : Grid.GetLength(1);

            for (int x = Border ? 1 : 0; x < x_stop; x++)
            {
                for (int y = Border ? 1 : 0; y < y_stop; y++)
                {
                    Grid[x, y] = Char;
                    BackgroundGrid[x, y] = BGColor;
                    ForegroundGrid[x, y] = FGColor;
                }
            }
        }

        internal string ReadLine()
        {
            StringBuilder sb = new StringBuilder();
            for (int x = 0; x < Cursor.X; x++ )
            {
                sb.Append(Grid[x, Cursor.Y]);
            }
            return sb.ToString();
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
          
            int x_stop = Border ? Grid.GetLength(0) - 1 : Grid.GetLength(0);
            int y_stop = Border ? Grid.GetLength(1) - 1 : Grid.GetLength(1);

            for (int x = Border?1:0; x < x_stop; x++)
            {
                for (int y = Border?1:0; y < y_stop; y++)
                {
                    Grid[x, y] = 0;
                    BackgroundGrid[x, y] = BGColor;
                    ForegroundGrid[x, y] = FGColor;
                }
            }
            Cursor = new Point(Border ? 1 : 0, Border ? 1 : 0);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            //   - Draw background grid -
            for (int x = 0; x < Grid.GetLength(0); x++)
            {
                for (int y = 0; y < Grid.GetLength(1); y++)
                {
                    spriteBatch.Draw(font, new Rectangle(x * CharSize + X, y * CharSize + Y, CharSize, CharSize),
                                     new Rectangle(11*CharSize, 13*CharSize, 10, 10),BackgroundGrid[x,y]);
                }
            }

            //    - Draw foreground grid -
            for (int x = 0; x < Grid.GetLength(0); x++)
            {
                for (int y = 0; y < Grid.GetLength(1); y++)
                {
                    
                    int x_source = Grid[x, y] % 16 * fontSize;
                    int y_source = Grid[x, y] / 16 * fontSize;
                    spriteBatch.Draw(font, new Rectangle(x * CharSize + X, y * CharSize + Y, CharSize, CharSize),
                        new Rectangle(x_source, y_source, 10, 10), ForegroundGrid[x,y]);
                }
            }

         
        }
    }
}