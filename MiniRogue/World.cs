using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace MiniRogue
{
    internal class World
    {
        internal Display infoPanel;
        internal Display display;
        internal StateManager Manager;
        internal List<Actor> Actors;
        internal List<Item> Features;
      
        public World(StateManager Manager)
        {
            this.Manager = Manager;
           
            Actors = new List<Actor>();
            Features = new List<Item>();

            display = new Display(Manager.game.Content.Load<Texture2D>("cp437T"),
                                  Manager.game.Content.Load<Texture2D>("White"),
                0,
                0,
                Manager.game.GraphicsDevice.Viewport.Width,
                Manager.game.GraphicsDevice.Viewport.Height - 40);

            infoPanel = new Display(
                Manager.game.Content.Load<Texture2D>("cp437T"),
                 Manager.game.Content.Load<Texture2D>("White"),
                10,
                Manager.game.GraphicsDevice.Viewport.Height - 40,
                Manager.game.GraphicsDevice.Viewport.Width - 20,
                40);

            infoPanel.SetCharSize(20);
            infoPanel.SetForegroundColor(Color.Red);
            infoPanel.SetBackgroundColor(Color.Yellow);

            infoPanel.Write("Test");
           

            Actor player = new Actor(2, 10, 10);
            player.Components.Add(new MovementContoller(player, this));
            Actors.Add(player);
            MakeRoom();

        }

       

        private void MakeRoom()
        {
            display.SetCharSize(20);
            string[] layout = new string[14] {"#########################",
                                              "#  [---]                #",
                                              "#  T                    #",
                                              "#                       #",
                                              "#                       #",
                                              "#                       #",
                                              "##################      #",
                                              "#                       #",
                                              "#   hT                  #",
                                              "#                       #",
                                              "#                       #",
                                              "#                       #",
                                              "#                       #",
                                              "############//###########" };

            for (int y = 0; y < layout.Length; y++)
            {
                for (int x = 0; x < layout[y].Length; x++)
                {
                    bool block = false;
                    switch (layout[y][x])
                    {
                        case '#': { block = true; break; }
                        case '[': { block = true; break; }
                        case ']': { block = true; break; }
                        case 'T': { block = true; break; }
                    }
                    Features.Add(new Item(layout[y][x], x, y, block));
                }
            }
        }

        public void Update(GameTime gameTime)
        {
           
            {
                foreach (Actor actor in Actors) { actor.Update(gameTime); }
              
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            display.Clear();
            foreach (Item feature in Features) { feature.Draw(display); }
            foreach (Actor actor in Actors) { actor.Draw(display); }
            display.Draw(spriteBatch);
            infoPanel.Draw(spriteBatch);

           
        }
    }
}