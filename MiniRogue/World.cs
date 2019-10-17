using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace MiniRogue
{
    internal class World
    {
        internal Display display;
        StateManager Manager;
        internal List<Actor> Actors;
       internal List<Item> Features;

        public World(StateManager Manager)
        {
            this.Manager = Manager;
            Actors = new List<Actor>();
            Features = new List<Item>();
            display = new Display(Manager.game.Content.Load<Texture2D>("cp437T"), 0, 0,
                            Manager.game.GraphicsDevice.Viewport.Width,
                            Manager.game.GraphicsDevice.Viewport.Height);
            display.CharSize = 20;

            Actor player = new Actor(2, 10, 10);
            player.Components.Add(new MovementContoller(player, this));
            Actors.Add(player);

            Features.Add(new Item('#', 5, 5));
        }

        public void Update(GameTime gameTime)
        {
            foreach (Actor actor in Actors) { actor.Update(gameTime); }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            display.Clear();
            foreach (Actor actor in Actors) { actor.Draw(display); }
            foreach (Item feature in Features ) { feature.Draw(display); }
            display.Draw(spriteBatch);
        }
    }
}