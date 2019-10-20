using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MiniRogue
{
    public class PlayState : State
    {
        internal World world;
        internal StoryManager storyManager;

        public PlayState(StateManager Manager):base(Manager)
        {
            world = new World(Manager);
            storyManager = new StoryManager(this);
            storyManager.Enqueue(new StartHappening(5));
        }


        public override void Update(GameTime gameTime)
        {
            world.Update(gameTime);
            storyManager.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            world.Draw(spriteBatch);
        }
    }
}
