using System;
using Microsoft.Xna.Framework.Graphics;

namespace MiniRogue
{
    internal class Renderer
    {
        private World world;
        private Display display;

        internal Display infoPanel;
        internal Display lookPanel;

        public int DisplayWidth { get { return (display.Width / display.CharSize) - 1; } }
        public int DisplayHeight { get { return (display.Height / display.CharSize) - 1; } }

        public Renderer(State state)
        {

            display = new Display(state.Manager.game.Content.Load<Texture2D>("cp437T"),
                                  0, 0, 
                                  state.Manager.game.GraphicsDevice.Viewport.Width - 200,
                                  state.Manager.game.GraphicsDevice.Viewport.Height - 100,
                                  true);

            display.SetCharSize(20);

            infoPanel = new Display(
               state.Manager.game.Content.Load<Texture2D>("cp437T"),

               0,
               state.Manager.game.GraphicsDevice.Viewport.Height - 100,
               state.Manager.game.GraphicsDevice.Viewport.Width,
               100, true);



            lookPanel = new Display(
               state.Manager.game.Content.Load<Texture2D>("cp437T"),

               state.Manager.game.GraphicsDevice.Viewport.Width - 200,
               0,
               200,
                state.Manager.game.GraphicsDevice.Viewport.Height - 100, true);

         
        }

        public void SetWorld(World world)
        {
            this.world = world;
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            world.Camera.TakeSnapshot(display);
            display.Draw(spriteBatch);
            infoPanel.Draw(spriteBatch);
            lookPanel.Draw(spriteBatch);
            display.Clear();           
        }
    }
}