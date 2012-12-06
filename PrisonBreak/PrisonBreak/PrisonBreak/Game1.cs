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
			GraphicsDevice.BlendState = BlendState.AlphaBlend;

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

			GameObject glassFront = GameObject.CreateStaticGO(GraphicsDevice, Content.Load<Texture2D>("Glass"), SpriteTransparency.Transparent);
			glassFront.Transform.Translate(new Vector3(0f, 20f, 50f));
			manager.AddGameObject(glassFront);

			GameObject glassBack = GameObject.CreateStaticGO(GraphicsDevice, Content.Load<Texture2D>("Glass"), SpriteTransparency.Transparent);
			glassBack.Transform.Translate(new Vector3(100f, 20f, -100f));
			manager.AddGameObject(glassBack);

			GameObject player = new GameObject();
			player.AddTransform();
			player.Transform.Translate(new Vector3(-200f, 50f, 0f));
			player.AddAnimation(Content.Load<Texture2D>("Kid"), new Vector2(33, 33));
			player.Animation.AddAnimation("idle", 0, 1);
			player.Animation.AddAnimation("run", 1, 2);
			player.AddRenderer(GraphicsDevice, SpriteTransparency.Transparent);
			player.AddDynamicRigidBody(new Vector2(33f, 33f));
			player.RigidBody.CollisionCategory = CollisionCats.PlayerCategory;
			//player.AddTrigger(new Vector2(16f, 5f));
			player.AddScript(new PlayerScript(player));
			manager.AddGameObject(player);

			GameObject player2 = new GameObject();
			player2.AddTransform();
			player2.Transform.Parent = player.Transform;
			player2.Transform.Translate(new Vector3(-200f, 100f, 0f));
			player2.AddAnimation(Content.Load<Texture2D>("Kid"), new Vector2(33, 33));
			player2.Animation.AddAnimation("idle", 0, 1);
			player2.Animation.Play("idle");
			player2.AddRenderer(GraphicsDevice, SpriteTransparency.Transparent);
			manager.AddGameObject(player2);

			GameObject ground = GameObject.CreateStaticPhysicsGO(GraphicsDevice, Content.Load<Texture2D>("DebugGround"), SpriteTransparency.Opaque);
			manager.AddGameObject(ground);

			GameObject camera = new GameObject();
			camera.AddTransform();
			camera.Transform.Z = 1000f;
			camera.AddCamera(GraphicsDevice.Viewport, true);
			camera.AddScript(new CameraScript(camera));
			manager.AddGameObject(camera);

			GameObject trigger = new GameObject();
			trigger.AddTransform();
			trigger.AddStaticSprite(Content.Load<Texture2D>("DebugVolume"));
			trigger.AddRenderer(GraphicsDevice, SpriteTransparency.Transparent);
			trigger.AddTrigger(new Vector2(50f, 50f));
			trigger.Trigger.OnEnter += new FarseerPhysics.Dynamics.OnCollisionEventHandler(Trigger_OnEnter);
			trigger.Trigger.OnExit += new FarseerPhysics.Dynamics.OnSeparationEventHandler(Trigger_OnExit);
			trigger.Trigger.CollidesWith = CollisionCats.PlayerCategory | CollisionCats.EnemyCategory;
			manager.AddGameObject(trigger);
		}

		void Trigger_OnExit(FarseerPhysics.Dynamics.Fixture fixtureA, FarseerPhysics.Dynamics.Fixture fixtureB)
		{
			Console.WriteLine("Exit!");
		}

		bool Trigger_OnEnter(FarseerPhysics.Dynamics.Fixture fixtureA, FarseerPhysics.Dynamics.Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
		{
			Console.WriteLine("Enter!");
			return false;
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

			manager.Render();

			base.Draw(gameTime);
			//RigidBody.DebugRender();
		}
	}
}
