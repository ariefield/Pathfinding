using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PathFinding
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        // Enum
        enum Tile
        {
            Open,
            Blocked,
            Start,
            Goal
        }

        // Consts
        const int WINDOW_WIDTH = 800;
        const int WINDOW_HEIGHT = 480;
        const int TILE_LENGTH = 40;

        // Properties
        static int TileRows => WINDOW_HEIGHT / TILE_LENGTH;
        static int TileCols => WINDOW_WIDTH / TILE_LENGTH;

        // Monogame related vars
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // Textures
        Texture2D t; //base for the line texture

        // Other vars
        Tile[,] grid = new Tile[TileRows, TileCols];

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                IsFullScreen = false,
                PreferredBackBufferWidth = WINDOW_WIDTH,
                PreferredBackBufferHeight = WINDOW_HEIGHT
            };
            graphics.ApplyChanges();

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Monogame init
            IsMouseVisible = true;

            // User defined init
            // Init grid
            for (int r = 0; r < TileRows; r++)
            {
                for (int c = 0; c < TileCols; c++)
                {
                    grid[r, c] = Tile.Open;
                }
            }

            grid[1, 3] = Tile.Blocked;

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

            // Create 1x1 texture for line drawing
            t = new Texture2D(GraphicsDevice, 1, 1);
            t.SetData(new Color[] { Color.White });// fill the texture with white
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);

            spriteBatch.Begin();

            DrawTiles(spriteBatch);
            DrawGridLines(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        // Non-monogame related functions
        private void DrawGridLines(SpriteBatch spriteBatch)
        {
            Console.WriteLine($"TileRows: { TileRows }");
            Console.WriteLine($"TileCols: { TileCols }");

            // Draw horizontal gridlines
            for (int r = 0; r <= TileRows; r++)
            {
                int y = r * TILE_LENGTH;
                DrawLine(spriteBatch, new Vector2(0, y), new Vector2(WINDOW_WIDTH, y));
            }

            // Draw vertical gridlines
            for (int c = 0; c <= TileCols; c++)
            {
                int x = c * TILE_LENGTH;
                DrawLine(spriteBatch, new Vector2(x, 0), new Vector2(x, WINDOW_HEIGHT));
            }
        }

        private void DrawTiles(SpriteBatch spriteBatch)
        {
            // Draw horizontal gridlines
            for (int r = 0; r < TileRows; r++)
            {
                for (int c = 0; c < TileCols; c++)
                {
                    int x = c * TILE_LENGTH;
                    int y = r * TILE_LENGTH;
                    Color color = Color.White;
                    if (grid[r, c] == Tile.Open)
                        color = Color.White;
                    else if (grid[r, c] == Tile.Blocked)
                        color = Color.Gray;

                    spriteBatch.Draw(t, new Rectangle(x, y, TILE_LENGTH, TILE_LENGTH), null, color);
                }
            }

        }


        private void DrawLine(SpriteBatch spriteBatch, Vector2 start, Vector2 end)
        {
            Vector2 edge = end - start;

            // Calculate angle to rotate line
            float angle = (float)Math.Atan2(edge.Y, edge.X);

            spriteBatch.Draw(t,
                new Rectangle(// rectangle defines shape of line and position of start of line
                    (int)start.X,
                    (int)start.Y,
                    (int)edge.Length(), //sb will strech the texture to fill this rectangle
                    1), //width of line, change this to make thicker line
                null,
                Color.Black, //colour of line
                angle,     //angle of line (calulated above)
                new Vector2(0, 0), // point in line about which to rotate
                SpriteEffects.None,
                0);
        }
    }
}
