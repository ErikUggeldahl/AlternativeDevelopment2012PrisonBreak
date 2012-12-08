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
			int width = debugRoomTex.Width;
			int height = debugRoomTex.Height;
			int tileSize = 10;
			sbyte[,] bytes = new sbyte[301, 301];
			uint[] texData = new uint[debugRoomTex.Width * debugRoomTex.Height];
			debugRoomTex.GetData(texData);
			for (int y = 0; y < debugRoomTex.Height; y++)
			{
				for (int x = 0; x < debugRoomTex.Width; x++)
				{
					if (texData[y * debugRoomTex.Width + x] == 0)
						bytes[x, y] = 1;
					else
						bytes[x, y] = -1;
				}
			}

			for (int y = 0; y < debugRoomTex.Height; y += tileSize)
			{
				for (int x = 0; x < debugRoomTex.Width; x += tileSize)
				{
					int currentX = x;
					while (currentX < debugRoomTex.Width && bytes[currentX, y] == -1)
					{
						currentX++;
					}

					if (currentX == x)
						continue;

					currentX -= x;
					Body tileBody = BodyFactory.CreateRectangle(RigidBody.World, currentX / RigidBody.MInPx, tileSize / RigidBody.MInPx, 0);
					Vector2 transform = new Vector2(x / RigidBody.MInPx, y / RigidBody.MInPx);
					tileBody.SetTransform(transform, 0f);

					x += currentX;
					currentX /= tileSize;
					Console.WriteLine(currentX);
				}
			}
			// Consider using above data without decomposition. Check right position, compose largest horizontal bodies. More optimal.

			int cellSize = 10;
			float subCellSize = 1f;
			int nXCells = width / cellSize;
			int nYCells = height / cellSize;

			List<Body>[,] bodyGrid = new List<Body>[nXCells, nYCells];

			for (int gridY = 0; gridY < nYCells; gridY++)
			{
				for (int gridX = 0; gridX < nXCells; gridX++)
				{
					float adjustedX = gridX * cellSize;
					float adjustedY = gridY * cellSize;
					List<Vertices> polys = MarchingSquares.DetectSquares(new AABB(new Vector2(adjustedX, adjustedY), new Vector2(adjustedX + cellSize, adjustedY + cellSize)), subCellSize, subCellSize, bytes, 2, true);
					bodyGrid[gridX, gridY] = new List<Body>();

					Vector2 scale = new Vector2(1f / RigidBody.MInPx, -1f / RigidBody.MInPx);
					Vector2 translate = new Vector2(((float)width / -2f) / RigidBody.MInPx, ((float)150f / 2f) / RigidBody.MInPx);
					for (int i = 0; i < polys.Count; i++)
					{
						polys[i].Scale(ref scale);
						polys[i].Translate(ref translate);
						polys[i].ForceCounterClockWise();
						Vertices verts = FarseerPhysics.Common.PolygonManipulation.SimplifyTools.CollinearSimplify(polys[i]);
						List<Vertices> decomposedPolys = EarclipDecomposer.ConvexPartition(verts);

						for (int j = 0; j < decomposedPolys.Count; j++)
						{
							//if (decomposedPolys[i].Count > 2)
								//bodyGrid[gridX, gridY].Add(BodyFactory.CreatePolygon(RigidBody.World, decomposedPolys[i], 1));
						}
					}
				}
			}

			RigidBody.DebugLoadContent(GraphicsDevice, Content);

			manager = new GameObjectManager();

			//GameObject glassFront = GameObject.CreateStaticGO(GraphicsDevice, Content.Load<Texture2D>("Glass"), SpriteTransparency.Transparent);
			//glassFront.Transform.Translate(new Vector3(0f, 20f, 50f));
			//manager.AddGameObject(glassFront);

			//GameObject glassBack = GameObject.CreateStaticGO(GraphicsDevice, Content.Load<Texture2D>("Glass"), SpriteTransparency.Transparent);
			//glassBack.Transform.Translate(new Vector3(100f, 20f, -100f));
			//manager.AddGameObject(glassBack);

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
