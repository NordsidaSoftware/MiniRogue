using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniRogue
{
    public class Camera
    {
        int X;
        int Y;
        int Width;
        int Height;
      

        public Rectangle InnerRectangle
        {
            get
            {
                return new Rectangle(X + (int)(Width * 0.5), Y + (int)(Height * 0.5),
                                     (int)(Width * 0.25), (int)(Height * 0.25) );
            }
        }

        World world;

        internal Camera(World world, int Width, int Height ) 
        {
            this.world = world;
            this.Width = Width;
            this.Height = Height;
        }

        internal void Move(int dx, int dy)
        {
            if (X + dx >= 0 && Y + dy >= 0)
            {
                if (X + Width + dx < world.Map.Width &&
                    Y + Height + dy < world.Map.Height)
                {
                    X += dx;
                    Y += dy;
                }
            }
        }

        internal bool IsOnCamera(int x, int y)
        {
            if (x > 0 && y > 0)
            {
                if (x < Width && y < Height)
                {
                    return true;
                }
            }
            return false;
        }

        internal void TakeSnapshot(Display display)
        {
            for ( int XPos = 1; XPos < Width; XPos++)
            {
                for (int YPos = 1; YPos < Height; YPos++)
                {
                    if (world.Map.tileGrid[X+XPos, Y+YPos] is Unknown) { continue; }
                    if (!world.Map.IsInFOV(X + XPos, Y + YPos)) { continue; }
                    Color? lightAmountOnTile = world.Map.GetLightAmount(X + XPos, Y + YPos);

                    int currentTile = world.Map.tileGrid[X+XPos, Y+YPos].Glyph;
                    Color FGColor = world.Map.tileGrid[X + XPos, Y + YPos].Color;
                    Color BGColor = world.Map.tileGrid[X + XPos, Y + YPos].BGColor;

                    if (world.Map.tileGrid[X + XPos, Y + YPos] is Empty) { BGColor = Color.Lerp(lightAmountOnTile.Value, BGColor, 0.3f); }
                    else { BGColor = Color.Lerp(lightAmountOnTile.Value, BGColor, 0.7f); }
                    display.SetChar(XPos, YPos, currentTile, FGColor, BGColor);
                    
                    foreach (Actor actor in world.Map.Actors)
                    {
                        if ( IsOnCamera(actor.X - X, actor.Y - Y ) &&
                                        world.Map.IsInFOV(actor.X, actor.Y ))
                        { actor.Draw(display, X, Y); }
                    }

                    world.FXManager.Draw(display, X, Y);
                }
            }
        }
    }
}
