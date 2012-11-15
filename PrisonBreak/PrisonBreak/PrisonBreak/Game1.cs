using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using PrisonBreak.Components;
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
		GameObjectManager manager;

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			graphics.PreferredBackBufferWidth = 1280;
			graphics.PreferredBackBufferHeight = 720;

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

			RigidBody.DebugLoadContent(GraphicsDevice, Content);

			manager = new GameObjectManager();

			GameObject player = new GameObject();
			player.AddTransform();
			player.Transform.Translate(new Vector2(30f, -0f));
			player.AddRenderer(spriteBatch);
			player.AddAnimation(Content.Load<Texture2D>("Kid"), new Rectangle(0, 0, 33, 33));
			player.Animation.AddAnimation("idle", 0, 1);
			player.Animation.AddAnimation("run", 1, 2);
			player.AddDynamicRigidBody(new Vector2(33f, 33f));
			player.AddScript(new PlayerScript(player));
			manager.AddGameObject(player);

			GameObject camera = new GameObject();
			camera.AddTransform();
			camera.AddCamera(GraphicsDevice.Viewport, true);
			camera.AddScript(new CameraScript(camera));
			manager.AddGameObject(camera);

			GameObject ground = new GameObject();
			ground.AddTransform();
			ground.Transform.Translate(new Vector2(640f, 650f));
			ground.AddRenderer(spriteBatch);
			ground.AddAnimation(Content.Load<Texture2D>("Ground"), new Rectangle(0, 0, 1280, 10));
			ground.Animation.AddAnimation("idle", 0, 1);
			ground.Animation.Play("idle");
			ground.AddStaticRigidBody(new Vector2(1280f, 10f));
			manager.AddGameObject(ground);
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
		protected override void Update(GameTime gameTime)
		{
			// Allows the game to exit
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
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

			spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, Camera.MainCamera.ViewMatrix);
			manager.Render();
			spriteBatch.End();

			RigidBody.DebugRender();
		}
	}
}
