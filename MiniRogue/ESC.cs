using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MiniRogue
{
    internal class ESC
    {
        internal bool Disabled;
        internal Actor Owner;
        public ESC(Actor Owner) { this.Owner = Owner; }
        public virtual void Update(GameTime gameTime) { }
    }

    internal class LightSource : ESC
    {
        private World world;
        public int radius;
        public Color Color;
     
        public LightSource(Actor Owner, World world, int radius, Color Color) : base(Owner)
        {
            this.world = world;
            this.radius = radius;
            this.Color = Color;
         
        }

        public override void Update(GameTime gameTime)
        {
            
                world.Map.visibility.RecalculateLightMap(Owner.X, Owner.Y, radius, Color);
        }
    
    }
    internal class CanComputeFOV : ESC
    {
        private World world;
        public int radius;
       
        public CanComputeFOV(Actor Owner, World world, int radius) : base(Owner)
        {
            this.world = world;
            this.radius = radius;
        }

        public override void Update(GameTime gameTime)
        {
            // Recalculate FOV if Owner moved in the last turn.
            if (Owner.HasMoved)
                world.Map.visibility.RecalculateFOV(Owner.X, Owner.Y, radius);
        }
    }
    internal class CameraSticky : ESC
    {
        internal World world;
        public CameraSticky(Actor Owner, World world) : base(Owner)
        {
            this.world = world;
        }

        public override void Update(GameTime gameTime)
        {
            if (Owner.X < world.Camera.InnerRectangle.Left)
            {
                world.Camera.Move(-1, 0);
            }
            if (Owner.X > world.Camera.InnerRectangle.Right)
            {
                world.Camera.Move(1, 0);
            }
            if (Owner.Y < world.Camera.InnerRectangle.Top)
            {
                world.Camera.Move(0, -1);
            }
            if (Owner.Y > world.Camera.InnerRectangle.Bottom)
            {
                world.Camera.Move(0, 1);
            }
        }
    }
    internal class SolidBody : ESC
    {
        internal bool IsGrounded;
        internal World world;

        public SolidBody(Actor Owner, World world) : base(Owner)
        { this.world = world; }
        public override void Update(GameTime gameTime)
        {
            if (!world.Map.IsSolid(Owner.X, Owner.Y+1)) { IsGrounded = false; }
            else { if (IsGrounded == false ) { IsGrounded = true; } }
            if (!IsGrounded)
            {
                ApplyGravity();
            }
        }

        private void ApplyGravity()
        {
            if ( world.Map.IsInBounds(Owner.X, Owner.Y+1) ){ Owner.Y += 1; }
        }
    }
    internal class ThrowController : ESC
    {
        World world;
        Point facing;
        public ThrowController(Actor Owner, World world) : base(Owner)
        {
            this.world = world;
            this.facing = new Point(0, 0);
        }

        public override void Update(GameTime gameTime)
        {
            if (InputHandler.WasKeyPressed(Keys.Up))   { facing = new Point(0, -1); }
            if (InputHandler.WasKeyPressed(Keys.Down)) { facing = new Point(0, 1); }
            if (InputHandler.WasKeyPressed(Keys.Left)) { facing = new Point(-1, 0); }
            if (InputHandler.WasKeyPressed(Keys.Right)){ facing = new Point(1, 0); }

            if (InputHandler.WasKeyPressed(Keys.Enter))
            {
                int targetX = Owner.X + facing.X*10;
                int targetY = Owner.Y + facing.Y*10;

             
                if (world.Map.IsInBounds(targetX, targetY))
                {
                    world.FXManager.Add(new LobTrace(world.FXManager, Owner.X, Owner.Y, targetX, targetY));
                    world.FXManager.Add(new Explosion(world.FXManager, targetX, targetY, 5, 50));
                }
               
            }    
        }
    }
    internal class DamageController : ESC
    {
        int MaxHP;
        int currentHP;
        World world;
        public DamageController(Actor Owner, World world, int MaxHP) : base(Owner)
        {
            this.MaxHP = MaxHP;
            this.currentHP = MaxHP;
            this.world = world;
        }

        public override void Update(GameTime gameTime)
        {
            int damage = world.Map.GetDamageAt(Owner.X, Owner.Y);
            if ( damage > 0 )
            {
                currentHP -= damage;
            }
            if (currentHP <= 0 )
            {
                Owner.Alive = false;
            }
            if (currentHP > MaxHP ) { currentHP = MaxHP; }
        }
    }
    internal class MovementController : ESC
    {
        World world;
        Point facing;
        public MovementController(Actor Owner, World world) : base(Owner){ this.world = world; }

        public override void Update(GameTime gameTime)
        {
            if (InputHandler.WasKeyPressed(Keys.Up)) { Move(0, -1); }//Jump(); } intill videre...
            if (InputHandler.WasKeyPressed(Keys.Down)) { Move(0, 1); }
            if (InputHandler.WasKeyPressed(Keys.Left)) { facing = new Point(-1, 0); Move(-1, 0); }
            if (InputHandler.WasKeyPressed(Keys.Right)) { facing = new Point(1, 0); Move(1, 0); }
        }

        private void Move(int dx, int dy)
        {
            
            if (!world.Map.IsInBounds( Owner.X+dx, Owner.Y + dy)) { return; }

            if (world.Map.IsSolid(Owner.X+dx, Owner.Y+dy)) { return; }

            Owner.X += dx;
            Owner.Y += dy;
            Owner.HasMoved = true;
            
        }

        private void Jump()
        {
            foreach (ESC cmp in Owner.Components)
            {
                if (cmp is SolidBody) { SolidBody cm = (SolidBody)cmp;
                    if ( cm.IsGrounded)
                    {
                        Move(facing.X, -2);
                    }
                }
            }
        }
    }
    internal class LookCursorController : ESC
    {
        World world;
        CanUseLookCommand LD;
    
        public LookCursorController(Actor Owner, World world, CanUseLookCommand LD) : base (Owner)
        {
            this.world = world;
            this.LD = LD;
        }
        public override void Update(GameTime gameTime)
        {
            if (InputHandler.WasKeyPressed(Keys.Space))
            {
                for ( int index = 0; index < LD.Owner.Components.Count; index++)
                {
                    if (LD.Owner.Components[index] is MovementController)
                    {
                        LD.Owner.Components[index].Disabled = false;
                    }
                    else if (LD.Owner.Components[index] is CanUseLookCommand)
                    {
                        LD.Owner.Components[index].Disabled = false;
                    }
                }
                Owner.Alive = false;
            }
            world.Render.lookPanel.Clear();
            world.Render.lookPanel.FGColor = Color.Red;
            world.Render.lookPanel.WriteLine("You are looking at: ");
            world.Render.lookPanel.WriteLine("");
            world.Render.lookPanel.FGColor = Color.Blue;

            Item item = world.Map.GetItemAt(Owner.X, Owner.Y);
            if (item != null)
            {
                world.Render.lookPanel.WriteLine(item.ToString());
            }

            List<Actor> actors = world.Map.GetActorsAt(Owner.X, Owner.Y);
            if (actors.Count > 0 )
            {
                foreach (Actor actor in actors)
                {
                    if (actor != Owner )
                        world.Render.lookPanel.WriteLine(actor.ToString());
                }
            }

            world.Render.lookPanel.WriteLine(world.Map.tileGrid[Owner.X, Owner.Y].ToString());
        }
    }
    internal class CanUseLookCommand : ESC
    {
        World world;
       
        public CanUseLookCommand(Actor Owner, World world) : base(Owner)
        {
            this.world = world;
        }

        public override void Update(GameTime gameTime)
        {
           if (InputHandler.WasKeyPressed(Keys.L))
            {
                Actor LookPoint = new Actor('X', Owner.X, Owner.Y);
                LookPoint.color = Color.Red;
                LookPoint.Components.Add(new MovementController(LookPoint, world));
                LookPoint.Components.Add(new LookCursorController(LookPoint, world, this));

                
                world.Map.Actors.Add(LookPoint);

                
                for ( int index = 0; index < Owner.Components.Count; index++)
                {
                    if (Owner.Components[index] is MovementController) { Owner.Components[index].Disabled = true; }
                    else if (Owner.Components[index] is CanUseLookCommand) { Owner.Components[index].Disabled = true; }
                }    
            }
        }
    }
    internal class RandomWalkAIMovementController : ESC
    {
        internal World world;
        internal float UpdateTimer;
        internal float WaitInterval = 200.0f;
        public RandomWalkAIMovementController(Actor Owner, World world):base(Owner)
        {
            this.world = world;
        }
        public override void Update(GameTime gameTime)
        {
            UpdateTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (UpdateTimer >= WaitInterval)
            {
                UpdateTimer -= WaitInterval;
                int randX = Randomizer.rnd.Next(-1, 2);
                int randY = Randomizer.rnd.Next(-1, 2);

                Move(randX, randY);
            }
        }

        private void Move(int dx, int dy)
        {

            if (Owner.X + dx < 1 || Owner.X + dx >= world.Map.Width) { return; }

            if (Owner.Y + dy < 1 || Owner.Y + dy >= world.Map.Height) { return; }
            if (world.Map.IsSolid(Owner.X + dx, Owner.Y + dy)) { return; }

            Owner.X += dx;
            Owner.Y += dy;
            Owner.HasMoved = true;
        }
    }
}