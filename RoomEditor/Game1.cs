using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;

namespace RoomEditor
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Display Display;
        Display CharSelector;
        Palette Palette;
        Display TextWindow;
        Random rnd;
        bool Saved;
        

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            IsMouseVisible = true;
            Content.RootDirectory = "Content";
            rnd = new Random();


        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            Display = new Display(Content.Load<Texture2D>("cp437T"), 0, 0, 500, 400, true);
            CharSelector = new Display(Content.Load<Texture2D>("cp437T"), 501, 0, 190, 190, true);
            Palette = new Palette(Content.Load<Texture2D>("cp437T"), 501, 201, 200, 200, true);
            TextWindow = new Display(Content.Load<Texture2D>("cp437T"), 0, 401, 500, 50, true);


            Palette.SetCharSize(20);
            CharSelector.SetCharSize(10);
            Palette.setupGrid();
            CharSelector.CurrentChar = 219;
            Palette.CurrentForeground = Color.Red;
            TextWindow.Write(Display.TileWidth.ToString() + "," + Display.TileHeight.ToString());


            for (int index = 0; index < 255; index++)
            {
                Point p = new Point(index % (CharSelector.TileWidth - 2),
                    index / (CharSelector.TileHeight - 2));

                CharSelector.SetChar(p.X + 1, p.Y + 1, index);
            }

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            InputHandler.Update();


            if (InputHandler.IsRMousePressed())
            {
                if (Display.Rectangle.Contains(InputHandler.MousePosition))
                {
                    Point point = InputHandler.MousePosition;
                    point.X = point.X / Display.CharSize;
                    point.Y = point.Y / Display.CharSize;
                    Display.SetChar(point,
                                     CharSelector.CurrentChar,
                                     Palette.CurrentForeground);
                }

                if (Palette.Rectangle.Contains(InputHandler.MousePosition))
                {
                    Point point = InputHandler.MousePosition;
                    point.X = point.X / Palette.CharSize - Palette.X / Palette.CharSize;
                    point.Y = point.Y / Palette.CharSize - Palette.Y / Palette.CharSize; ;
                    Palette.SetColor(point.X, point.Y);
                }

                if (CharSelector.Rectangle.Contains(InputHandler.MousePosition))
                {
                    Point point = InputHandler.MousePosition;
                    point.X = point.X / CharSelector.CharSize - CharSelector.X / CharSelector.CharSize;
                    point.Y = point.Y / CharSelector.CharSize - CharSelector.Y / CharSelector.CharSize; ;
                    CharSelector.CurrentChar = CharSelector.GetChar(point.X, point.Y);
                }
            }

            if (InputHandler.IsKeyPressed(Keys.LeftControl)&&
                InputHandler.IsKeyPressed(Keys.F)) { Display.Fill(219); Saved = false; }

            if (InputHandler.IsKeyPressed(Keys.LeftControl) &&
                InputHandler.IsKeyPressed(Keys.S)&& !Saved) { Export(); }

            if (InputHandler.WasKeyPressed(Keys.Left)) { Rescale(-1, 0); }
            if (InputHandler.WasKeyPressed(Keys.Right)) { Rescale(1, 0); }
            if (InputHandler.WasKeyPressed(Keys.Up)) { Rescale(0, -1); }
            if (InputHandler.WasKeyPressed(Keys.Down)) { Rescale(0, 1); }

            if (InputHandler.WasKeyPressed(Keys.Escape)) { Exit(); }




            base.Update(gameTime);
        }

        private void Rescale(int dx, int dy)
        {
            Display.Rescale(dx, dy);
            TextWindow.Clear();
            TextWindow.Write(Display.TileWidth.ToString() + "," + Display.TileHeight.ToString());
        }


        private void Export()
        {
            Saved = true;
            int Width = Display.TileWidth;
            int Height = Display.TileHeight;
            int DataBuffer = 2;
            int randomnumber = rnd.Next();
            string FileName = Width.ToString() + Height.ToString() + randomnumber.ToString() + ".MRE";


            byte[] SaveChar = new byte[(Width * Height) + DataBuffer];


            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    SaveChar[x + (y * Width)] = (byte)Display.Grid[x + 1, y + 1];
                }
            }
            SaveChar[(Width * Height)] = (byte)Width;
            SaveChar[(Width * Height) + 1] = (byte)Height;
            File.WriteAllBytes(@"C:\Users\kroll\Documents\MREditor\" + FileName, SaveChar);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp,
                              null, null, null, null);

            Display.Draw(spriteBatch);
            CharSelector.Draw(spriteBatch);
            Palette.Draw(spriteBatch);
            TextWindow.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
