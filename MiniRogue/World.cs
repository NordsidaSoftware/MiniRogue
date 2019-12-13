using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace MiniRogue
{
    internal class World
    {
        
        internal StateManager Manager;
       
        internal Map Map;
        
        internal Camera Camera;

        internal FXManager FXManager;

        internal Renderer Render;
      
      
        public World(StateManager Manager, int DisplayWidth, int DisplayHeight, Renderer Render)
        {
            this.Manager = Manager;
            Map = new Map(300, 500);
            Camera = new Camera(this, DisplayWidth, DisplayHeight);                         
            FXManager = new FXManager(this);
            this.Render = Render;
           
            CreateNewLevel();
        }

        private void CreateNewLevel()
        {
            Mapper mp = new Mapper(this);
            mp.GenerateMap();


        //  +++ DOWN STAIRS : NOT IMPLEMENTED +++
        Point? DownStairs = Map.FindEmptyTile();
            if (DownStairs != null) { Map.AddItem(new DStair(DownStairs.Value.X, DownStairs.Value.Y)); }

            Actor player = new Actor(2, mp.Layout[0].Rectangle.Center.X, mp.Layout[0].Rectangle.Center.Y)
            {
                color = Color.Red
            };
            player.Components.Add(new MovementController(player, this));
            player.Components.Add(new ThrowController(player, this));
            player.Components.Add(new CanUseLookCommand(player, this));
           // player.Components.Add(new SolidBody(player, this));
            player.Components.Add(new CameraSticky(player, this));
            player.Components.Add(new CanComputeFOV(player, this, 7));
            player.Components.Add(new LightSource(player, this, 10, Color.LightGoldenrodYellow));
            Map.Actors.Add(player);

            Actor NPC = new Actor(1, mp.Layout[0].Rectangle.Center.X+1, mp.Layout[0].Rectangle.Center.Y+1)
            {
                color = Color.Yellow
            };
            NPC.Components.Add(new RandomWalkAIMovementController(NPC, this));
            NPC.Components.Add(new DamageController(NPC, this, 5));
            NPC.Components.Add(new LightSource(NPC, this, 5, Color.LightBlue));
            Map.Actors.Add(NPC);
        }



        public void Update(GameTime gameTime)
        {
           // FXManager.Update(gameTime);  -disabled for now...
            Map.Update(gameTime);
           
        }

        public void Draw(SpriteBatch spriteBatch)
        {
          
             
        
        }
    }
}