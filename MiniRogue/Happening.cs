using System;
using Microsoft.Xna.Framework;

namespace MiniRogue
{
    internal class Happening
    {
        public double timer;
        public PlayState playState;

        public Happening(int timer) { this.timer = timer; }
        internal virtual void Update(GameTime gameTime)
        {
            timer -= gameTime.ElapsedGameTime.TotalSeconds;
            if (timer < 0 )
            {
                playState.storyManager.Dequeue().Execute();
            }
        }

        internal virtual void Execute() { }
    }


    internal class StartHappening : Happening
    {
        public StartHappening(int timer) : base(timer)  { }

        internal override void Execute()
        {
            playState.world.infoPanel.Write("Hør ni gjøkorna?");
            playState.storyManager.Enqueue(new MiddleHappening(5));
        }
    }

    internal class MiddleHappening : Happening
    {
        public MiddleHappening(int timer) : base(timer)
        {
        }

        internal override void Execute()
        {
            playState.world.infoPanel.Write("Ock så her hørs dem ut");
        }
    }
}