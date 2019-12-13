using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace MiniRogue
{
    internal class Line
    {
        public Point Start { get; private set; }
        public Point Stop { get; private set; }


        public Line(Point start, Point end)
        {
            this.Start = start;
            this.Stop = end;
        }


        //================= BRESENHAM LINE ALGORITHM  =======================
        public List<Point> GetPoints()
        {
            List<Point> linePoints = new List<Point>();
           
            int x = Start.X;
            int y = Start.Y;
            int dx = Math.Abs(Stop.X - Start.X);
            int sx = Start.X < Stop.X ? 1 : -1;
            int dy = -Math.Abs(Stop.Y - Start.Y);
            int sy = Start.Y < Stop.Y ? 1 : -1;
            int err = dx + dy;
            int e2;

            for (; ; )
            {
               
                linePoints.Add(new Point(x, y));
                if (x == Stop.X && y == Stop.Y) break;
              
                e2 = 2 * err;
                if (e2 >= dy ) { err += dy; x += sx; }
                if (e2 <= dx ) { err += dx; y += sy; }
            }

            return linePoints;
        }
    }
}