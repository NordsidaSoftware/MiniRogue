using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MiniRogue
{
    internal class ESC
    {
        internal Actor Owner;
        public ESC(Actor Owner) { this.Owner = Owner; }
        public virtual void Update(GameTime gameTime) { }
    }

    internal class MovementContoller : ESC
    {
        World world;
        public MovementContoller(Actor Owner, World world) : base(Owner){ this.world = world; }

        public override void Update(GameTime gameTime)
        {
            if (InputHandler.WasKeyPressed(Keys.Up)) { Move(0, -1); }
            if (InputHandler.WasKeyPressed(Keys.Down)) { Move(0, 1); }
            if (InputHandler.WasKeyPressed(Keys.Left)) { Move(-1, 0); }
            if (InputHandler.WasKeyPressed(Keys.Right)) { Move(1, 0); }
        }

        private void Move(int dx, int dy)
        {
            foreach (Item i in world.Features) { if (i.X == Owner.X+dx && i.Y == Owner.Y + dy) { return; } }
            Owner.X += dx;
            Owner.Y += dy;
        }
    }
}