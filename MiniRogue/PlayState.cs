using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MiniRogue
{
    public class PlayState : State
    {
        World world;

        public PlayState(StateManager Manager):base(Manager)
        {
            world = new World(Manager);
        }

        public override void Update(GameTime gameTime)
        {
            world.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            world.Draw(spriteBatch);
        }
    }
}
