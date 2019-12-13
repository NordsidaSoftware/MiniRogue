using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MiniRogue
{
    public class PlayState : State
    {
        internal World world;
        internal Renderer Render;


        public PlayState(StateManager Manager):base(Manager)
        {
            Render = new Renderer(this);
            world = new World(Manager, Render.DisplayWidth, Render.DisplayHeight, Render);
            Render.SetWorld(world);
        }


        public override void Update(GameTime gameTime)
        {
            world.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Render.Draw(spriteBatch);
        }
    }
}
