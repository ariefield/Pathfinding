using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PathFinding.src;
//using 

namespace PathFinding
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class MainLoop : Game
    {
        // Consts
        const int WINDOW_WIDTH = 800;
        const int WINDOW_HEIGHT = 480;
        const int TILE_LENGTH = 40;
        static Color GRID_COLOR = Color.Gray;
        static int TileRows => WINDOW_HEIGHT / TILE_LENGTH;
        static int TileCols => WINDOW_WIDTH / TILE_LENGTH;

        // Monogame related vars
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // Textures
        Texture2D t; //base for the line texture

        // Other vars
        Tile[,] Tiles;
        Search Search;
        Dictionary<Keys, bool> Pressed;

        public MainLoop()
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

            // Init tiles
            Tiles = new Tile[TileRows, TileCols];
            for (int r = 0; r < TileRows; r++)
            {
                for (int c = 0; c < TileCols; c++)
                {
                    Tiles[r, c] = new Tile(TileType.Open);
                }
            }

            for (int r = 0; r < TileRows; r++)
            {
                for (int c = 0; c < TileCols; c++)
                {
                    Tile tile = Tiles[r, c];

                    // Up
                    if (r > 0)
                        tile.Neighbours.Add(Tiles[r - 1, c]);

                    // Down
                    if (r < TileRows - 1)
                        tile.Neighbours.Add(Tiles[r + 1, c]);

                    // Left
                    if (c > 0)
                        tile.Neighbours.Add(Tiles[r, c - 1]);

                    // Right
                    if (c < TileCols - 1)
                        tile.Neighbours.Add(Tiles[r, c + 1]);
                }
            }

            Tiles[5, 3].Type = TileType.Start;
            Tiles[5, 16].Type = TileType.Goal;

            Search = new Search(Tiles[5, 3], Tiles[5, 16]);

            Pressed = new Dictionary<Keys, bool> {
                { Keys.Space, false},
                { Keys.Right, false}
            };

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

            if (!Search.Searching)
            {
                if (UserPressed(Keys.Space))
                {
                    Search.StartSearch(SearchType.Bfs);
                    Search.AutoAdvance = true;
                }

                if (UserPressed(Keys.Right))
                {
                    Search.StartSearch(SearchType.Bfs);
                    Search.Update();
                }

            }
            else
            {
                if (Search.AutoAdvance)
                {
                    if (UserPressed(Keys.Space))
                        Search.AutoAdvance = false;
                    else
                        Search.Update();
                }

                else if (UserPressed(Keys.Space))
                    Search.AutoAdvance = true;

                else if (UserPressed(Keys.Right))
                    Search.Update();
            }

            foreach (Keys key in Pressed.Keys.ToList())
            {
                Pressed[key] = Keyboard.GetState().IsKeyDown(key);
            }


            base.Update(gameTime);
        }


        #region UserInput

        private bool UserPressed(Keys key)
        {
            if (Keyboard.GetState().IsKeyDown(key) && !Pressed[key])
                return true;
            else
                return false;
        }

        #endregion

        #region Drawing

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

        private void DrawGridLines(SpriteBatch spriteBatch)
        {
            // Draw horizontal gridlines
            for (int r = 0; r <= TileRows; r++)
            {
                int y = r * TILE_LENGTH;
                DrawLine(spriteBatch, new Vector2(0, y), new Vector2(WINDOW_WIDTH, y), GRID_COLOR);
            }

            // Draw vertical gridlines
            for (int c = 0; c <= TileCols; c++)
            {
                int x = c * TILE_LENGTH;
                DrawLine(spriteBatch, new Vector2(x, 0), new Vector2(x, WINDOW_HEIGHT), GRID_COLOR);
            }
        }

        private void DrawTiles(SpriteBatch spriteBatch)
        {
            for (int r = 0; r < TileRows; r++)
            {
                for (int c = 0; c < TileCols; c++)
                {
                    int x = c * TILE_LENGTH;
                    int y = r * TILE_LENGTH;
                    Tile tile = Tiles[r, c];

                    spriteBatch.Draw(t, new Rectangle(x, y, TILE_LENGTH, TILE_LENGTH), null, tile.GetColor(Search));
                }
            }
        }

        private void DrawLine(SpriteBatch spriteBatch, Vector2 start, Vector2 end, Color color, int width = 1)
        {
            Vector2 edge = end - start;

            // Calculate angle to rotate line
            float angle = (float)Math.Atan2(edge.Y, edge.X);

            spriteBatch.Draw(t,
                new Rectangle(
                    (int)start.X,
                    (int)start.Y,
                    (int)edge.Length(), // Stretch texture to fill rectangle
                    width),
                null,
                color,
                angle,
                new Vector2(0, 0), // Origin of rotation
                SpriteEffects.None,
                0);
        }
    }

    #endregion
}
