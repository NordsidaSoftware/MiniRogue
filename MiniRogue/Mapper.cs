using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace MiniRogue
{

    /// <summary>
    /// Mapper class is responsible for the generation of the cave system map
    /// </summary>
    internal class Mapper
    {
        private World world;
        public List<Room> Layout = new List<Room>();
        private int timer;
        List<string> prefabRoomFiles = new List<string>();

       

    public Mapper(World world)
        {
            this.world = world;
            timer = 0;
            //                 Initialize the prefab room list by reading all files and storing
            //                 them in prefabRoomFiles.
            foreach (string file in Directory.EnumerateFiles(@"C: \Users\kroll\Documents\MREditor", "*.MRE"))
            {
                prefabRoomFiles.Add(file);
            }

          
        }

        internal void GenerateMap()
        {
            world.Map.Initialize();

            List<Socket> s = new List<Socket>();
            s.Add(new Socket(SocketType.Right, new Point(5, 5)));

            Room start = new Room(6, 6, s);
            start.Place(10, 10);
            Layout.Add(start);

            GenerateNode(start);
          
            CarveLayoutInMap();

            RenderMapLayout();
        }

        private void RenderMapLayout()
        {
            for (int y = 1; y < world.Map.Height-1; y++)
            {
                for (int x = 1; x < world.Map.Width-1; x++)
                {
                    
                    Tile current = world.Map.tileGrid[x, y];
                    if (current is Empty || current is Unknown) { continue; }
                    bool setToUnknown = true;
                    foreach (Tile neighbor in CurrentTileNeightbors(x, y))
                    {
                        if (neighbor is Empty) { setToUnknown = false; }
                    }
                    if (setToUnknown) { world.Map.tileGrid[x, y] = new Unknown(); }
                }
            }
        }

        private List<Tile> CurrentTileNeightbors(int x, int y)
        {
            List<Tile> returnTiles = new List<Tile>();
            List<Point> neighbors = new List<Point>()
            { new Point(0, -1),new Point(0, 1), new Point(-1, 0), new Point(1, 0),
              new Point(1, 1), new Point(-1, -1), new Point(1, -1), new Point(-1, 1) };

            foreach (Point p in neighbors)
            {
                returnTiles.Add(world.Map.tileGrid[x + p.X, y + p.Y]);
            }
            return returnTiles;
        }

        public void GenerateNode(Room node)
        {
            timer++;
            if (timer > 1000 ) { return; }
            Room next = GetRandomPrefabRoom();
            Point Connection = new Point();

            if (Match())
            {
                Layout.Add(next);
            }
         
           else { next = Layout[Randomizer.rnd.Next(0, Layout.Count)]; }

            if (Layout.Count < 100)
                GenerateNode(next);

            bool Match()
            {
                for ( int indexA = 0; indexA < node.Sockets.Count; indexA++)
                {
                    Socket A = node.Sockets[indexA];
                    if (A.Occupied) { continue; }
                    for ( int indexB = 0; indexB < next.Sockets.Count; indexB++)
                    {
                        Socket B = next.Sockets[indexB];
                        if (A.Type == B.Opposite && !B.Occupied)
                        {
                            Connection = new Point();
                            switch (A.Type)
                            {
                                case SocketType.Down: { Connection = new Point(A.Position.X - B.Position.X, A.Position.Y+1); break; }
                                case SocketType.Up: { Connection = new Point(A.Position.X - B.Position.X, A.Position.Y-next.Height ); break; }
                                case SocketType.Left: { Connection = new Point(A.Position.X-next.Width, A.Position.Y - B.Position.Y); break; }
                                case SocketType.Right: { Connection = new Point(A.Position.X+1, A.Position.Y-B.Position.Y); break; }
                            }

                            next.Place(Connection.X, Connection.Y);
                            if (!Intersects(next) &&
               world.Map.IsInBounds(next.Rectangle.X, next.Rectangle.Y) &&
               world.Map.IsInBounds(next.Rectangle.X + next.Rectangle.Width,
                                    next.Rectangle.Y + next.Rectangle.Height))
                            {
                                A.Occupied = true;
                                B.Occupied = true;

                                return true;
                            }
                            next.ReSet();
                        }
                    }
                }
                return false;
            }
        }
       
        public bool Intersects(Room node)
        {
            foreach (Room oldNode in Layout)
            {
                if (oldNode.Rectangle.Intersects(node.Rectangle)) { return true; }
            }
            return false;
        }

        public Room GetRandomPrefabRoom()
        {
            byte[] lines = File.ReadAllBytes(prefabRoomFiles[Randomizer.rnd.Next(0, prefabRoomFiles.Count)]);
            int Width = lines[lines.Length-2];
            int Height = lines[lines.Length-1];

            List<Socket> sockets = new List<Socket>();


            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    byte c = lines[x + (y * Width)];

                    if (c==24) // UpArrow
                    {
                        sockets.Add(new Socket(SocketType.Up, new Point(x, y)));
                    }
                    if (c== 25) // DownArrow
                    {
                        sockets.Add(new Socket(SocketType.Down, new Point(x, y)));
                    }
                    if (c==26) // RightArrow
                    {
                        sockets.Add(new Socket(SocketType.Right, new Point(x, y)));
                    }
                    if( c==27) // LeftArrow
                    {
                        sockets.Add(new Socket(SocketType.Left, new Point(x, y)));
                    }
                }
            }

            Room prefab = new Room(Width, Height, sockets);
            Tile tile;
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    byte c = lines[x + (y * Width)];
                    switch (c)
                    {
                        case 0: { tile = new Empty(); break; }
                        case 219: { tile = new Rock(); break; }
                        default: { tile = new Empty(); break; }
                    }
                    prefab.Grid[x, y] = tile;
                }
            }
    
            return prefab;
        }
       

        public void CarveLayoutInMap()
        {
            foreach (Room room in Layout)
            {
                
                for (int x = 0; x < room.Rectangle.Width; x++)
                {
                    for (int y = 0; y < room.Rectangle.Height; y++)
                    {
                        world.Map.tileGrid[room.Rectangle.X + x + 1, room.Rectangle.Y + y + 1] =
                                                                                             room.Grid[x, y];
                        

                        // HACK : entrance room grid is not defined yet...
                        // SUPER HACKY : will set floor tiles to rock. 
                        if (room.Grid[x,y] == null ) {
                            world.Map.tileGrid[room.Rectangle.X + x + 1, room.Rectangle.Y + y +2] = new Rock();
                            world.Map.tileGrid[room.Rectangle.X + x + 1, room.Rectangle.Y + y + 1] = new Empty(); }
                       
                    }
                }
            }
        }
    }

    public class Room
    {
        public Rectangle Rectangle;
        public Tile[,] Grid;
        public int Width;
        public int Height;
        public List<Socket> Sockets;
        

        public Room(int Width, int Height,List<Socket> sockets)
        {
            Sockets = sockets;
            this.Width = Width; this.Height = Height;
            Grid = new Tile[Width, Height]; 
        }

     
        public Socket GetASocket()
        {
            return Sockets[Randomizer.rnd.Next(0, Sockets.Count)];
        }

        public void Place(int x, int y)
        {
            Rectangle = new Rectangle(x, y, Width, Height);
            foreach (Socket s in Sockets)
            {
                s.Position = new Point(s.Position.X + x, s.Position.Y + y);
            }
        }

        public void ReSet()
        {
            foreach (Socket s in Sockets)
            {
                s.Position = new Point(s.Position.X - Rectangle.X, s.Position.Y - Rectangle.Y);
            }
        }
    }

    public enum SocketType { Up, Down, Left, Right }

    public class Socket
    {
        public SocketType Type;
        public Point Position;
        public SocketType Opposite;
        public bool Occupied;
        public Socket(SocketType type, Point position)
        {
            Type = type;
            Position = position;
            if (Type == SocketType.Down) { Opposite = SocketType.Up; }
            else if (Type == SocketType.Up) { Opposite = SocketType.Down; }
            else if (Type == SocketType.Left) { Opposite = SocketType.Right; }
            else if (Type == SocketType.Right) { Opposite = SocketType.Left; }
        }
    }
}