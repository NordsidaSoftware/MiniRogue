using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace MiniRogue
{
    /// <summary>
    /// Class responsible for all the grid data 
    /// </summary>
    internal class Map
    {
        public int Width;    // Width of map in tiles
        public int Height;   // Height of map in tiles
        public List<Actor> Actors; // TODO : move to other class ?
        public List<Item> Items;   // TODO : move to other class ?
    
        public byte[,] damageGrid;
        public Tile[,] tileGrid;
        public Visibility visibility;

        public Map(int Width, int Height)
        {
            Actors = new List<Actor>();
            Items = new List<Item>();
            this.Width = Width;
            this.Height = Height;
            damageGrid = new byte[Width, Height];
           
            tileGrid = new Tile[Width, Height];
            visibility = new Visibility(this);
        }


        //          ===============      METHODS     =============Get====
        internal void Initialize()
        {
            for ( int x = 0; x < Width; x++)
            {
                for ( int y = 0; y < Height; y++)
                {
                    tileGrid[x, y] = new Unknown();
                }
            }
        }
        internal bool IsSolid(int x, int y)
        {
            if (IsInBounds(x, y))
            {
               if (tileGrid[x,y].Blocked) { return true; }
                else return false;
            }
            return true;
        }
        internal bool IsInBounds(int x, int y)
        {
            if (x > 0 && y > 0)
            {
                if (x < Width-1 && y < Height -1)
                {
                    return true;
                }
            }
            return false;
        }

        internal bool IsInFOV(int x, int y)
        {
            return visibility.GetInFov(x, y);
        }

        internal Color? GetLightAmount(int x, int y)
        {
            if (!IsInBounds(x, y)) { return null; }
            return visibility[x, y];
        }
       
        internal void AddItem(Item item)
        {
            Items.Add(item);
        }
        internal Point? FindEmptyTile()
        {
            // REmoved intil videre :_)
            return null;
        }
        internal int GetDamageAt(int x, int y)
        {
            if (x > 0 && y > 0)
            {
                if(x < Width && y < Height)
                {
                    int damage = damageGrid[x, y];
                    damageGrid[x, y] = 0;
                    return damage;
                }
            }
            return 0;
        }
        public void ClearDamageGrid()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    damageGrid[x, y] = 0;
                }
            }
        }
        internal List<Actor> GetActorsAt(int x, int y)
        {
            List<Actor> actors = new List<Actor>();
            foreach (Actor actor in Actors)
            {
                if (actor.X == x && actor.Y == y ) { actors.Add(actor); }
            }
            return actors;
        }
        internal Item GetItemAt(int x, int y)
        {
           foreach (Item item in Items)
            {
                if (item.X == x && item.Y == y ) { return item; }
            }
            return null;
        }
        internal void DestroyAllAt(int x, int y)
        {
           // Removed intil videre...:)
        }


        internal void Update(GameTime gameTime)
        {
            // ClearDamageGrid();
            visibility.InitializeColorGrid();

            for (int actorIndex = 0; actorIndex < Actors.Count; actorIndex++)
            {
                // Update all ESC in current Actor:
                Actors[actorIndex].Update(gameTime);

                // Reset moved flag :
                if (Actors[actorIndex].HasMoved) { Actors[actorIndex].HasMoved = false; }

                // Remove dead actors :
                if (!Actors[actorIndex].Alive) { Actors.RemoveAt(actorIndex); }
            }
        }

    }
}
