using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace MiniRogue
{
    internal class ParticleEffect
    {
        internal List<Particle> particles;
        internal FXManager Manager;
        internal bool DestroysBackground;

        public ParticleEffect(FXManager Manager)
        {
            particles = new List<Particle>();
            this.Manager = Manager;
        }
        
        public int ParticleCount { get { return particles.Count; } }

        internal void Update(GameTime gameTime)
        {
            foreach (Particle particle in particles)
            {
                particle.Update(gameTime);
            }

            for ( int index = 0; index < particles.Count; index++ )
            {
                Particle p = particles[index];
                if (!p.IsAlive) { if (DestroysBackground) Manager.world.Map.DestroyAllAt(p.X, p.Y); particles.Remove(p); }
            }
        }

        internal void AddParticle(Particle p)
        {
          if (Manager.world.Map.IsInBounds(p.X, p.Y))
            particles.Add(p);
        }

        internal void Draw(Display display)
        {
            foreach (Particle particle in particles)
            {
                particle.Draw(display);
            }


        }
        internal void Draw(Display display, int x, int y)
        {
            foreach (Particle particle in particles)
            {
                particle.Draw(display, x, y);
            }
        }
        internal virtual void OnFinished() {  }
       
    }

    internal class Particle
    {
        public int X;
        public int Y;
        public int Glyph;
        public Color color;
        public Color StartColor;
        public Color EndColor;
        public bool ColorChange;
        public bool GlyphChange;

      
        public float TotalTime;
        public bool IsAlive;
        public int TTD { get; internal set; }   // Time To Die !
        public int TTL { get; internal set; }   // Time To Live !

        public Particle(int X, int Y, int Glyph, Color color, Color EndColor, int TTD, int TTL) : this(X, Y, Glyph, color, EndColor, TTD)
        {
            this.TTL = TTL;
        }
        public Particle(int X, int Y, int Glyph, Color color, Color EndColor, int TTD) : this(X, Y, Glyph, color )
        { ColorChange = true;  this.StartColor = color; this.EndColor = EndColor; TotalTime = TTD; this.TTD = TTD; }
        public Particle(int X, int Y, int Glyph, Color color)
        {
            this.X = X;
            this.Y = Y;
            this.Glyph = Glyph;
            this.color = color;
            this.TTD = TTL;
            IsAlive = true;

           
        }

        internal void Update(GameTime gameTime)
        {
            if (TTL > 0) { TTL--; }  // If before TTL, countdown
                                     // else do lifetime update
            else
            {
                TTD--;
                if (TTD <= 0) { IsAlive = false; }

                if (ColorChange)
                {
                    color = Color.Lerp(EndColor, StartColor, TTD / TotalTime);
                }

                if (GlyphChange)
                {
                    Glyph = Randomizer.rnd.Next(0, 255);
                }

            }
           
        }
        internal void Draw(Display display)
        {
            if (TTL <= 0)
                display.SetChar(X, Y, Glyph, color);
        }

        internal void Draw(Display display, int x, int y)
        {
            if (TTL <= 0)
                display.SetChar(X - x, Y - y, Glyph, color);
        }

    }
    internal class SmokeRing :ParticleEffect
    {
        public SmokeRing (FXManager Manager, int x , int y, int radius, int timer = 0):base(Manager)
        {
            int currentRadius = 0;
            for (int time = timer; time < radius+ timer; time++)
            {
                currentRadius++;
                Circle c = new Circle(x, y, currentRadius);
                foreach (Point p in c.GetCircle()) { AddParticle(new Particle(p.X, p.Y, 219, Color.White, Color.White, 2, time)); }
            }
        }
    }
    internal class ShockRing : ParticleEffect
    {
        public ShockRing(FXManager Manager, int x, int y, int radius, int timer = 0) : base(Manager)
        {
            int currentRadius = 0;
            for (int time = timer; time < radius + timer; time++)
            {
                currentRadius++;
                Circle c = new Circle(x, y, currentRadius);
                foreach (Point p in c.GetCircle()) { AddParticle(new Particle(p.X, p.Y, 247, Color.Blue, Color.LightBlue, 1, time)); }
            }
        }
    }
    internal class Blast : ParticleEffect
    {
        int x, y, radius;
        public Blast(FXManager Manager, int x, int y, int radius, int timer = 0) : base(Manager)
        {
            Circle blast = new Circle(x, y, radius);
            Circle innerBlast = new Circle(x, y, radius / 2);
            List<Point> blastArea = blast.GetArea();
            List<Point> innerBlastArea = innerBlast.GetArea();

            this.x = x;
            this.y = y;
            this.radius = radius;

            DestroysBackground = true;

            for (int i = 0; i < blastArea.Count/3; i++)
            {
                Point randomPoint = blastArea[Randomizer.rnd.Next(0, blastArea.Count)];
                int TTL = Randomizer.rnd.Next(35);

                AddParticle(new Particle(randomPoint.X, randomPoint.Y, 219, Color.Red, Color.Yellow, TTL, timer+1));
            }
            for (int i = 0; i < innerBlastArea.Count/2; i++)
            {
                int TTL = Randomizer.rnd.Next(25) + 5;
                Point randomPoint = innerBlastArea[Randomizer.rnd.Next(0, innerBlastArea.Count)];
                AddParticle(new Particle(randomPoint.X, randomPoint.Y, 219, Color.White, Color.Orange, TTL, timer));
            }

        }

        internal override void OnFinished()
        {
            Circle damage = new Circle(x, y, radius);
            foreach (Point p in damage.GetArea())
            {
                if (Manager.world.Map.IsInBounds(p.X, p.Y))
                {
                    Manager.world.Map.damageGrid[p.X, p.Y] = (byte)(radius * 10);
                }

            }
        }
    }

    internal class LobTrace : ParticleEffect
    {
        public LobTrace(FXManager Manager, int startX, int startY, 
            int targetX, int targetY) : base(Manager)
        {
                int timer = 0;
                int dy = startY;

            if (startX < targetX)   // Left to Right
            {
                int halfway = (targetX + startX) / 2;
                for (int dx = startX; dx < targetX; dx++)
                {
                    timer += 3;
                    if (dx > halfway) { dy += 1; }
                    else { dy -= 1; }

                    AddParticle(new Particle(dx, dy, 'O', Color.Black, Color.Black, 1, timer));
                }

            }
            else if (startX > targetX) // Right to Left
            {
                int halfway = (startX - targetX) / 2;
                for (int dx = startX; dx > targetX; dx--)
                {
                    timer += 3;
                    if (dx > halfway+targetX) { dy -= 1; }
                    else { dy += 1; }

                    AddParticle(new Particle(dx, dy, 'O', Color.Black, Color.Black, 1, timer));
                }
            }
            else if (startY < targetY)  // Down
            {
                for (int Y = startY; Y < targetY; Y++)
                {
                    timer++;
                    AddParticle(new Particle(startX, Y, 'O', Color.Black, Color.Black, 1, timer));
                }
            }
            else if (startY > targetY)  // Up
            {
                for (int Y = startY; Y > targetY; Y--)
                {
                    timer++;
                    AddParticle(new Particle(startX, Y, 'O', Color.Black, Color.Black, 1, timer));
                }
            }
        }
    }
    internal class Explosion :ParticleEffect
    {
        public Explosion(FXManager Manager, int x, int y, int radius, int timer= 0):base(Manager)
        { 
           Manager.Add(new ShockRing(Manager, x, y, radius, timer));
           Manager.Add(new Blast(Manager, x, y, radius, timer+5));
           Manager.Add(new SmokeRing(Manager, x, y, radius, timer+10));

           
        }
    }
}
