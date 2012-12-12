using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using PrisonBreak.Components;
using PrisonBreak.Scripts;
using PrisonBreak.Scripts.AI;

using FarseerPhysics.Collision;
using FarseerPhysics.Common;
using FarseerPhysics.Common.Decomposition;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

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

		public const int ResolutionWidth = 1280;
		public const int ResolutionHeight = 720;

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			graphics.PreferredBackBufferWidth = ResolutionWidth;
			graphics.PreferredBackBufferHeight = ResolutionHeight;

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

			GameObject mainLevel = WorldGen.CreateWorldGO(GraphicsDevice, Content, "Levels/LevelMain", "Levels/LevelMain");
			manager.AddGameObject(mainLevel);

			//GameObject glassFront = GameObject.CreateStaticGO(GraphicsDevice, Content.Load<Texture2D>("Glass"), SpriteTransparency.Transparent);
			//glassFront.Transform.Translate(new Vector3(0f, 20f, 50f));
			//manager.AddGameObject(glassFront);

			//GameObject glassMid = GameObject.CreateStaticGO(GraphicsDevice, Content.Load<Texture2D>("Glass"), SpriteTransparency.Transparent);
			//glassMid.Transform.Translate(new Vector3(600f, 0f, 0f));
			//manager.AddGameObject(glassMid);

			//GameObject glassBack = GameObject.CreateStaticGO(GraphicsDevice, Content.Load<Texture2D>("Glass"), SpriteTransparency.Transparent);
			//glassBack.Transform.Translate(new Vector3(0f, 20f, -100f));
			//manager.AddGameObject(glassBack);

            //GameObject fan = FanScript.CreateFanGO(Content, GraphicsDevice);
            //manager.AddGameObject(fan);
            //fan.Transform.Translate(new Vector3(300f, 30f, 0f));

			//GameObject laser = LaserScript.CreateLaserGO(Content, GraphicsDevice);
			//manager.AddGameObject(laser);
			//laser.Transform.Translate(new Vector3(400f, 30f, 0f));

			//List<Vector2> patrolPoints = GuardScript.CreatePatrolPoints(200f, -50f, 300f, -50f, 400f, -50f, 500f, -50f);
			//GameObject guard = GuardScript.CreateGuardGO(Content, GraphicsDevice, patrolPoints);
			//guard.Transform.Translate(new Vector2(-300f, -50f));
			//manager.AddGameObject(guard);

			GameObject player = new GameObject();
			player.AddTransform();
			player.Transform.Translate(new Vector3(-200f,-50f, 0f));
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
			player2.Transform.Translate(new Vector3(-200f, 100f, 0f));
			player2.Transform.Parent = player.Transform;
			
			player2.AddAnimation(Content.Load<Texture2D>("Kid"), new Vector2(33, 33));
			player2.Animation.AddAnimation("idle", 0, 1);
			player2.Animation.Play("idle");
			player2.AddRenderer(GraphicsDevice, SpriteTransparency.Transparent);
			manager.AddGameObject(player2);

			GameObject camera = new GameObject();
			camera.AddTransform();
			camera.Transform.Z = 600f;
			camera.AddCamera(GraphicsDevice.Viewport, true);
			camera.AddScript(new CameraScript(camera));
			manager.AddGameObject(camera);

			GameObject cameraBounds = GameObject.CreateStaticGO(GraphicsDevice, Content.Load<Texture2D>("DebugCameraBounds"), SpriteTransparency.Transparent);
			cameraBounds.Transform.Z = -600f;
			cameraBounds.Transform.Parent = camera.Transform;
			
			manager.AddGameObject(cameraBounds);

			//GameObject trigger = new GameObject();
			//trigger.AddTransform();
			//trigger.AddStaticSprite(Content.Load<Texture2D>("DebugVolume"));
			//trigger.AddRenderer(GraphicsDevice, SpriteTransparency.Transparent);
			//trigger.AddTrigger(new Vector2(50f, 50f));
			//trigger.Trigger.OnEnter += new FarseerPhysics.Dynamics.OnCollisionEventHandler(Trigger_OnEnter);
			//trigger.Trigger.OnExit += new FarseerPhysics.Dynamics.OnSeparationEventHandler(Trigger_OnExit);
			//trigger.Trigger.CollidesWith = CollisionCats.PlayerCategory | CollisionCats.EnemyCategory;
			//manager.AddGameObject(trigger);
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

		bool canDebugSwitch = true;
		protected override void Update(GameTime gameTime)
		{
			// Allows the game to exit
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				this.Exit();

			GameTimeGlobal.GameTime = gameTime;

			// Toggle debug physics view
			if (canDebugSwitch && Input.KeyboardState.IsKeyDown(Keys.NumPad0))
			{
				RigidBody.IsDebugEnabled = !RigidBody.IsDebugEnabled;
				canDebugSwitch = false;
			}
			if (!canDebugSwitch && Input.KeyboardState.IsKeyUp(Keys.NumPad0))
				canDebugSwitch = true;

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
			
			if (RigidBody.IsDebugEnabled)
				RigidBody.DebugRender();
		}
	}
}
