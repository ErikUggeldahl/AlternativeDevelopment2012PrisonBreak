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

using PrisonBreak.Scripts;

namespace PrisonBreak
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
		SpriteFont Text;
        GameObjectManager manager;
        GameObject player;

        public Game1()
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

            manager = new GameObjectManager();
            player = new GameObject();
            player.AddTransform();
            player.AddRenderer(spriteBatch);
            player.AddAnimation(Content.Load<Texture2D> ("Kid"), new Rectangle(0,0,33, 33));
            player.CAnimation.AddAnimation("idle", 0, 1);
            player.CAnimation.AddAnimation("run", 1, 2);
            player.AddScript(new PlayerScripts(player));
            manager.AddGameObject(player);
            player.CTransform.Translate(new Vector2(60f, -0f));

			string dialogueText;
			List<string> stringList = new List<string>();
			stringList.Add("Hi"); // Add string 1
			stringList.Add("this is a test"); // 2
			stringList.Add("this is a test"); // 3
			stringList.Add("this is a test"); // 4
			stringList.Add("this is a test"); // 5
			stringList.Add("this is a test"); // 5
			stringList.Add("this is a test"); // 5

			//dialogueText = string.Join(", ", stringList.ToArray());
			dialogueText = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vestibulum et feugiat arcu. Phasellus sed velit in sapien congue adipiscing. Nulla faucibus egestas ante gravida porttitor. Etiam sed odio est, eget vehicula justo. Phasellus sodales nulla consectetur eros imperdiet vulputate. Morbi iaculis lobortis leo id posuere. Nunc viverra interdum nunc a condimentum. Mauris id velit eu lectus viverra feugiat non ac.";
			player.AddDialogueBox(dialogueText, Content.Load<SpriteFont>("SpriteFont"), Content.Load<Texture2D>("Box"));

            GameObject camera = new GameObject();
            camera.AddTransform();
            camera.AddCamera(GraphicsDevice.Viewport, true);
            manager.AddGameObject(player);
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

            GameTimeGlobal.GameTime = gameTime;

            Input.Update();
            manager.Update();
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

            spriteBatch.Begin(SpriteSortMode.Immediate,null, null, null, null, null, Camera.MainCamera.viewMatrix);

            manager.Render();

            spriteBatch.End();
        }
    }
}
