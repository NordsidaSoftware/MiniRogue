using Microsoft.Xna.Framework;

namespace MiniRogue
{
    public class Tile
    {
        public bool Blocked { get; internal set; }
        public int Glyph { get; internal set; }
        public Color Color { get; internal set; }

        public Color BGColor { get; internal set; }
       

        public override string ToString()
        {
            return Glyph.ToString();
        }
    }

    public class Unknown : Tile
    {
       public Unknown () { Blocked = true; }
    }

    public class Empty : Tile
    {
        public Empty () { Blocked = false; Color = Color.Transparent; Glyph = ' '; }
    }

    public class Rock : Tile
    {
        public Rock () { Blocked = true; Color = Color.Brown; BGColor = Color.DarkSlateGray; Glyph = '='; }
    }

}