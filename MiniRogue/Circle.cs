using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniRogue
{
    public class Circle
    {
        public int OriginX { get; private set; }
        public int OriginY { get; private set; }
        public int Radius { get; private set; }

        public Circle(int OriginX, int OriginY, int Radius)
        {
            this.OriginX = OriginX;
            this.OriginY = OriginY;
            this.Radius = Radius;
        }

        public List<Point> GetCircle()
        {
            List<Point> CirclePoints = new List<Point>();
            int x = -Radius;
            int y = 0;
            int r = Radius;
            int err = 2 - 2 * Radius;

            do
            {
                CirclePoints.Add(new Point(OriginX - x, OriginY + y));
                CirclePoints.Add(new Point(OriginX - y, OriginY - x));
                CirclePoints.Add(new Point(OriginX + x, OriginY - y));
                CirclePoints.Add(new Point(OriginX + y, OriginY + x));
                r = err;
                if (r <= y) err += ++y * 2 + 1;
                if (r > x || err > y) err += ++x * 2 + 1;
            } while (x < 0);

            return CirclePoints;
        }

        public List<Point> GetArea()
        {
            List<Point> areaPoints = new List<Point>();
            int dX, dY;

            Rectangle boundingBox = new Rectangle(OriginX-Radius, OriginY-Radius, Radius * 2, Radius*2);
            for ( int x = boundingBox.X; x < OriginX+boundingBox.Width; x++)
            {
                for ( int y = boundingBox.Y; y < OriginY+boundingBox.Height; y++)
                {
                    dX = OriginX - x;
                    dY = OriginY - y;
                    if ((dX * dX ) + (dY*dY) <= (Radius * Radius)){ areaPoints.Add(new Point(x, y)); }
                }
            }

            return areaPoints;
            
        }

        

        public bool Intersects(Circle circle)
        {
            return true;
        }
    }
}
