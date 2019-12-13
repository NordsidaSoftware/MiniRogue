using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MiniRogue
{
    public class Visibility
    {
        private int width;
        private int height;
        private Color[,] LightGrid;
        private bool[,] fovGrid;

        

        private Map Map;

        public Color this [int x, int y] {  get { return LightGrid[x, y]; } }
      
        internal Visibility(Map map)
        {
            this.Map = map;
            this.width = map.Width;
            this.height = map.Height;
            LightGrid = new Color[width, height];
            fovGrid = new bool[width, height];

            InitializeColorGrid();
            ResetFovMap();
        }

        public void InitializeColorGrid()
        {
            for ( int x = 0; x < width; x++)
            {
                for ( int y = 0; y < height; y++)
                {
                    LightGrid[x, y] = new Color(10, 10, 50);
                }
            }
        }
        internal void RecalculateLightMap(int x, int y, int r, Color Color)
        {
          
            for (; r >= 0; r--)
            {
                Circle c = new Circle(x, y, r);
                foreach (Point point in c.GetArea())
                {
                    if (point.X <= 0 || point.Y <= 0 || point.X >= width || point.Y >= height) { continue; }
                  
                        LightGrid[point.X, point.Y] = LightenTile(LightGrid[point.X, point.Y], r, Color);
                }
            }
        }

        private Color LightenTile(Color originColor, int  intensity, Color lightColor)
        {
            
            Color blend = Color.Lerp(originColor, lightColor, 0.5f);
         
            return new Color(blend.R - intensity * 5,
                             blend.G - intensity * 5,
                             blend.B - intensity * 5);
                             

          
          
        }

        private void ResetFovMap()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    fovGrid[x, y] = false;
                }
            }
        }

        internal void RecalculateFOV(int x, int y, int r)
        {
            ResetFovMap();
         
            Rectangle rectangle = new Rectangle(x - r, y - r, r * 2, r * 2);
            Point origin = new Point(x, y);
            List<Point> Endpoints = rectangle.Walls();   // YAY! Extension Method !!!
           
            List<Line> Rays = new List<Line>();

            foreach (Point circlePoint in Endpoints)
            {
                Rays.Add(new Line(origin, circlePoint));
            }

            foreach (Line ray in Rays)
            {
                foreach (Point point in ray.GetPoints())
                {
                    if (point.X <= 0 || point.Y <= 0 || point.X >= width || point.Y >= height) { continue; }
                    

                    fovGrid[point.X, point.Y] = true;
                    if (Map.tileGrid[point.X, point.Y].Blocked) { break; }
                }
            }
        }

        internal bool GetInFov(int x, int y)
        {
            return fovGrid[x, y];
        }
    }
}