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

namespace PrisonBreak.Shading.ShadingDebug
{
    public class TestShadingGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D testSquare;
        Effect testEffect;

        SpriteFont debugFont;
        Dictionary<String, String> debugStatements;

        Stopwatch sWatch = new Stopwatch();

        public TestShadingGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            Window.Title = "Shading Testing";

            Console.WriteLine(GraphicsDevice.Viewport.Bounds);

            debugStatements = new Dictionary<String, String>();
            debugStatements.Add("FPS", "");
            debugStatements.Add("Frame time", "");
            debugStatements.Add("Update time", "");

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            testSquare = Content.Load<Texture2D>("DebugContent/ColouredSquare");
            testEffect = Content.Load<Effect>("DebugContent/TestEffect");

            debugFont = Content.Load<SpriteFont>("DebugContent/DebugFont");
        }

        protected override void UnloadContent()
        {
        }

        int interval = 0;
        bool keyDown = false;

        protected override void Update(GameTime gameTime)
        {
            sWatch.Start();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            MouseState mState = Mouse.GetState();

            KeyboardState kState = Keyboard.GetState();
            if (kState.GetPressedKeys().Length == 0)
            {
                keyDown = false;
            }
            if (!keyDown)
            {
                if (kState.IsKeyDown(Keys.A))
                {
                    keyDown = true;
                }
            }

            base.Update(gameTime);

            if (interval == 0)
                debugStatements["Update time"] = sWatch.ElapsedMilliseconds.ToString();
        }

        protected override void Draw(GameTime gameTime)
        {
            long startTime = sWatch.ElapsedMilliseconds;
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearWrap, DepthStencilState.Default, RasterizerState.CullClockwise, testEffect);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, testEffect);

            spriteBatch.Draw(testSquare, new Vector2(100f, 100f), testSquare.Bounds, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

            spriteBatch.End();

            spriteBatch.Begin();

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

