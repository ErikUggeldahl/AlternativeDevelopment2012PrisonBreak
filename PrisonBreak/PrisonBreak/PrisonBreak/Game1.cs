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

		Texture2D debugRoomTex;

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

			debugRoomTex = Content.Load<Texture2D>("DebugRoom");
			uint[] data = new uint[debugRoomTex.Width * debugRoomTex.Height];
			debugRoomTex.GetData(data);
			//Vertices verts = PolygonTools.CreatePolygon(data, debugRoomTex.Width, 0f, 0, true, true);
			List<Vertices> v = PolygonTools.CreatePolygon(data, debugRoomTex.Width, 100f, 0, false, false);
			//Vertices verts = v[0];
			List<Vector2> rawVerts = new List<Vector2>();
			rawVerts.Add(new Vector2(50f,50f));
			rawVerts.Add(new Vector2(50f,-50f));
			rawVerts.Add(new Vector2(-50f,-50f));
			rawVerts.Add(new Vector2(-50f,50f));
			rawVerts.Add(new Vector2(-25f, 25f));
			rawVerts.Add(new Vector2(-25f, -25f));
			rawVerts.Add(new Vector2(25f, -25f));
			rawVerts.Add(new Vector2(25f, 25f));

			rawVerts.Add(new Vector2(-25f, 25f));
			rawVerts.Add(new Vector2(-50f, 50f));
			//rawVerts.Add(new Vector2(-10f, 10f));
			//rawVerts.Add(new Vector2(10f, 10f));
			//rawVerts.Add(new Vector2(10f, -10f));
			//rawVerts.Add(new Vector2(-10f, -10f));

			sbyte[,] bytes = new sbyte[300, 300];
			for (int i = 0; i < 300; i++)
			{
				for (int j = 0; j < 300; j++)
				{
					if (j == 50 || j == 51)
					{
						bytes[i, j] = 0;
						break;
					}
					bytes[i, j] = 1;
				}
			}

			List<Vertices> v2 = MarchingSquares.DetectSquares(new FarseerPhysics.Collision.AABB(new Vector2(0f,0f), new Vector2(50f,50f)), 10f, 10f, bytes, 0, true);


			Vertices verts = new Vertices(rawVerts);
			//Vertices verts = PolygonTools.CreateRectangle(10f, 10f);
			Vector2 scale = new Vector2(1f/RigidBody.MInPx, 1f/RigidBody.MInPx);
			verts.Scale(ref scale);
			//List<Vertices> vertList = BayazitDecomposer.ConvexPartition(verts);
			//List<Vertices> vertList = CDTDecomposer.ConvexPartition(verts);
			List<Vertices> vertList = EarclipDecomposer.ConvexPartition(verts);
			//List<Vertices> vertList = FlipcodeDecomposer.ConvexPartition(verts);
			Body body = BodyFactory.CreateBody(RigidBody.World);
			List<Fixture> compound = FixtureFactory.AttachCompoundPolygon(v2, 1f, body);
			Vector2 pos = new Vector2(1f, 2f);
			body.SetTransform(ref pos, 0f);

			RigidBody.DebugLoadContent(GraphicsDevice, Content);

			manager = new GameObjectManager();

			GameObject glassFront = GameObject.CreateStaticGO(GraphicsDevice, Content.Load<Texture2D>("Glass"), SpriteTransparency.Transparent);
			glassFront.Transform.Translate(new Vector3(0f, 20f, 50f));
			manager.AddGameObject(glassFront);

			GameObject glassBack = GameObject.CreateStaticGO(GraphicsDevice, Content.Load<Texture2D>("Glass"), SpriteTransparency.Transparent);
			glassBack.Transform.Translate(new Vector3(100f, 20f, -100f));
			manager.AddGameObject(glassBack);

            GameObject fan = FanScript.CreateFanGO(Content, GraphicsDevice);
            manager.AddGameObject(fan);
            fan.Transform.Translate(new Vector3(300f, 30f, 0f));

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
