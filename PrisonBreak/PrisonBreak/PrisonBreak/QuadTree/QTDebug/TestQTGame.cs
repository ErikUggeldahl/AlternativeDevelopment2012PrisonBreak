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

using System.Diagnostics;

namespace PrisonBreak.QuadTree.Debug
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class TestQTGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D testSquare;
        Texture2D translucentSquare;

        QuadTree<IBounded> qt;

        DebugGO camera;
        DebugGO[] gos;

        SpriteFont debugFont;
        Dictionary<String, String> debugStatements;

        Stopwatch sWatch = new Stopwatch();
        bool drawQuads = false;
        bool useQuadTree = true;

        const int numberToTest = 10000;

        public TestQTGame()
        {
            graphics = new GraphicsDeviceManager(this);
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
            Window.Title = "QuadTree Testing";
            
            qt = new QuadTree<IBounded>(GraphicsDevice.Viewport.Bounds);

            camera = new DebugGO(new Rectangle(0, 0, 160, 90));
            gos = new DebugGO[numberToTest];
            Random r = new Random();
            for (int i = 0; i < gos.Length; i++)
            {
                gos[i] = new DebugGO(new Rectangle(r.Next(800 - 2), r.Next(480 - 2), 2, 2));
                qt.Insert(gos[i]);
            }

            Console.WriteLine(GraphicsDevice.Viewport.Bounds);

            debugStatements = new Dictionary<String, String>();
            debugStatements.Add("FPS", "");
            debugStatements.Add("Frame time", "");
            debugStatements.Add("Update time", "");
            debugStatements.Add("Query time", "");

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

            testSquare = Content.Load<Texture2D>("DebugContent/TestSquare");
            translucentSquare = Content.Load<Texture2D>("DebugContent/TranslucentSquare");

            debugFont = Content.Load<SpriteFont>("DebugContent/DebugFont");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>

        int interval = 0;
        bool keyDown = false;

        bool removing = false;
        int toRemove = 0;
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            sWatch.Start();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (removing)
            {
                if (toRemove < gos.Length)
                {
                    qt.Remove(gos[toRemove]);
                    toRemove++;
                }
            }

            MouseState mState = Mouse.GetState();
            camera.SetPos(new Vector2(mState.X, mState.Y));

            KeyboardState kState = Keyboard.GetState();
            if (kState.GetPressedKeys().Length == 0)
            {
                keyDown = false;
            }
            if (!keyDown)
            {
                if (kState.IsKeyDown(Keys.G))
                {
                    drawQuads = !drawQuads;
                    keyDown = true;
                }
                if (kState.IsKeyDown(Keys.Q))
                {
                    useQuadTree = !useQuadTree;
                    keyDown = true;
                }
                if (kState.IsKeyDown(Keys.D))
                {
                    removing = !removing;
                    keyDown = true;
                }
                if (kState.IsKeyDown(Keys.C))
                {
                    qt.Clear();
                    keyDown = true;
                }
            }

            if (kState.IsKeyDown(Keys.OemPlus))
            {
                camera.AddSize(1);
            }
            if (kState.IsKeyDown(Keys.OemMinus))
            {
                camera.AddSize(-1);
            }

            base.Update(gameTime);

            if (interval == 0)
                debugStatements["Update time"] = sWatch.ElapsedMilliseconds.ToString();
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            long startTime = sWatch.ElapsedMilliseconds;
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            if (drawQuads)
            {
                List<Rectangle> quadPrint = qt.Print();
                for (int i = 0; i < quadPrint.Count; i++)
                {
                    spriteBatch.Draw(translucentSquare, quadPrint[i], Color.White);
                }
            }

            spriteBatch.Draw(testSquare, camera.AABounds, new Color(0f, 0f, 0f, 0.5f));

            if (useQuadTree)
            {
                startTime = sWatch.ElapsedMilliseconds;
                List<IBounded> toRender = qt.Query(camera);
                if (interval == 0)
                    debugStatements["Query time"] = (sWatch.ElapsedMilliseconds - startTime).ToString();

                for (int i = 0; i < toRender.Count; i++)
                {
                    spriteBatch.Draw(testSquare, toRender[i].AABounds, Color.White);
                }
            }
            else
            {
                if (interval == 0)
                    debugStatements["Query time"] = "N/A";
                for (int i = 0; i < gos.Length; i++)
                {
                    spriteBatch.Draw(testSquare, gos[i].AABounds, Color.White);
                }
            }

            if (interval == 0)
            {
                startTime = sWatch.ElapsedMilliseconds;
                if (startTime < 1)
                    debugStatements["FPS"] = "> 1000";
                else
                    debugStatements["FPS"] = (1000f / (float)startTime).ToString();
                debugStatements["Frame time"] = sWatch.ElapsedMilliseconds.ToString();
            }
            interval++;
            interval %= 15;

            sWatch.Reset();
            for (int i = 0; i < debugStatements.Count; i++)
            {
                KeyValuePair<String, String> pair = debugStatements.ElementAt(i);
                spriteBatch.DrawString(debugFont, pair.Key + ": " + pair.Value, new Vector2(10f, 10f + 15f * i), Color.Black);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

