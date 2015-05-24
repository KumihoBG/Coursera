using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using TeddyMineExplosion;

namespace ProgrammingAssignment5
    {
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
        {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        const int WINDOW_WIDTH = 800;
        const int WINDOW_HEIGHT = 600;
        const int TOTAL_SPAWN_DELAY_MILLISECONDS = 1000;
        const float MIN_VALUE = -0.5f;
        const float MAX_VALUE = 0.5f;

        // teddy bear spawn support 
        int elapsedSpawnDelayMilliseconds = 0;
        static Random rand = new Random();

        static float result = MIN_VALUE + (float)rand.NextDouble() * (MAX_VALUE - MIN_VALUE);
        Vector2 velocity = new Vector2(result);
        Texture2D bearSprite;
        List<TeddyBear> bears = new List<TeddyBear>();

        // declaring sprites
        Texture2D mineSprite;
        Texture2D explosionSprite;

        // game objects
        List<Explosion> explosions = new List<Explosion>();
        List<Mine> mines = new List<Mine>();

        //click support
        ButtonState PreviousButtonState = ButtonState.Released;

        public Game1()
            {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //set resolution and make mouse visible
            graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
            graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
            IsMouseVisible = true;
            }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
            {
            // TODO: Add your initialization logic here

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
            bearSprite = Content.Load<Texture2D>("teddybear");
            mineSprite = Content.Load<Texture2D>("mine");
            explosionSprite = Content.Load<Texture2D>("explosion");
            elapsedSpawnDelayMilliseconds = rand.Next(1000, 3001);
            }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            MouseState mouse = Mouse.GetState();

            if (mouse.LeftButton == ButtonState.Released &&
                PreviousButtonState == ButtonState.Pressed)
                {
                mines.Add(new Mine(mineSprite, mouse.X, mouse.Y));
                }
            PreviousButtonState = mouse.LeftButton;

            // spawn teddies as appropriate
            elapsedSpawnDelayMilliseconds += gameTime.ElapsedGameTime.Milliseconds;
            if (elapsedSpawnDelayMilliseconds > TOTAL_SPAWN_DELAY_MILLISECONDS)
                {
                elapsedSpawnDelayMilliseconds = 0;
                bears.Add(new TeddyBear(bearSprite, velocity, WINDOW_WIDTH, WINDOW_HEIGHT));
                }

            // update and blow up teddy bears
            foreach (TeddyBear teddyBear in bears)
                {
                teddyBear.Update(gameTime);
                foreach (var mine in mines)
                    {
                    if (teddyBear.CollisionRectangle.Intersects(mine.CollisionRectangle))
                        {
                        teddyBear.Active = false;
                        mine.Active = false;
                        explosions.Add(new Explosion(explosionSprite,
                            teddyBear.CollisionRectangle.Center.X,
                            teddyBear.CollisionRectangle.Center.Y));
                        }
                    }
                }

            // update explosions
            foreach (Explosion explosion in explosions)
                {
                explosion.Update(gameTime);
                }

            // remove dead teddies
            for (int i = bears.Count - 1; i >= 0; i--)
                {
                if (!bears[i].Active)
                    {
                    bears.RemoveAt(i);
                    }
                }

            // remove dead explosions
            for (int i = explosions.Count - 1; i >= 0; i--)
                {
                if (!explosions[i].Playing)
                    {
                    explosions.RemoveAt(i);
                    }
                }

            // remove dead mines
            for (int i = mines.Count - 1; i >= 0; i--)
                {
                if (!mines[i].Active)
                    {
                    mines.RemoveAt(i);
                    }
                }

            base.Update(gameTime);
            }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
            {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            foreach (TeddyBear teddyBear in bears)
                {
                teddyBear.Draw(spriteBatch);
                }

            foreach (Mine mine in mines)
                {
                mine.Draw(spriteBatch);
                }

            foreach (Explosion explosion in explosions)
                {
                explosion.Draw(spriteBatch);
                }

            spriteBatch.End();

            base.Draw(gameTime);
            }
        }
    }