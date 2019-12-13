using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace MiniRogue
{
    internal class FXManager
    {
        private List<ParticleEffect> activeEffects;
        internal World world;
        public FXManager(World world)
        {
            this.world = world;
            activeEffects = new List<ParticleEffect>();
        }

        public void Update(GameTime gameTime)
        {
            foreach (ParticleEffect effect in activeEffects)
            {
                effect.Update(gameTime);
            }
            for ( int index = 0; index < activeEffects.Count; index++)
            {
                if (activeEffects[index].ParticleCount <= 0 )
                {
                    activeEffects[index].OnFinished();
                    activeEffects.RemoveAt(index);
                }
            }
        }

        internal void Add(ParticleEffect effect)
        {
            activeEffects.Add(effect);
        }

        internal void Draw(Display display)
        {
            foreach (ParticleEffect effect in activeEffects)
            {
                effect.Draw(display);
            }
        }

        internal void Draw(Display display, int x, int y)
        {
            foreach (ParticleEffect effect in activeEffects)
            {
                effect.Draw(display, x, y);
            }
        }
    }
}