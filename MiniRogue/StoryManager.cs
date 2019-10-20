using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MiniRogue
{
    internal class StoryManager
    {
        private PlayState playState;
        private Queue<Happening> Happenings;

        private bool StackHasElements {  get { return Happenings.Count > 0; } }

        public StoryManager(PlayState playState)
        {
            this.playState = playState;
            Happenings = new Queue<Happening>();
        }

        internal void Update(GameTime gameTime)
        {
            if (StackHasElements)
                Happenings.Peek().Update(gameTime);
        }

        public void Enqueue(Happening happening)
        {
            happening.playState = playState;
            Happenings.Enqueue(happening);
        }
        public Happening Dequeue()
        {
            return Happenings.Dequeue();
        }
    }
}