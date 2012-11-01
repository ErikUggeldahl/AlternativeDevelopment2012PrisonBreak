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

namespace PrisonBreak
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

   Texture2D ground;
   Texture2D dude;
   Camera myCamera;
   Vector2 characterPosition = Vector2.Zero;
      

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            GameObject g = new GameObject();
            g.AddComponent(new Audio());
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
            ground = Content.Load<Texture2D>("ground");
            dude = Content.Load<Texture2D>("dude");


            myCamera = new Camera(GraphicsDevice.Viewport);
            myCamera.Limits = new Rectangle(0, 0, 3200, 600);
          
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
            KeyboardState keyState = Keyboard.GetState();
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;


            if (keyState.IsKeyDown(Keys.Right))
            {
                characterPosition.X += 100.0f * elapsedTime;
            }
            if (keyState.IsKeyDown(Keys.Left))
            {
                characterPosition.X -= 100.0f * elapsedTime;
            }
            if (keyState.IsKeyDown(Keys.Down))
            {
                characterPosition.Y += 100.0f * elapsedTime;
            }
            if (keyState.IsKeyDown(Keys.Up))
            {
                characterPosition.Y -= 100.0f * elapsedTime;
            }

            myCamera.LookAt(characterPosition);
            
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

            spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, myCamera.viewMatrix);

            spriteBatch.Draw(ground, new Vector2(0, -600), Color.White);
            spriteBatch.Draw(dude, characterPosition, Color.White);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
