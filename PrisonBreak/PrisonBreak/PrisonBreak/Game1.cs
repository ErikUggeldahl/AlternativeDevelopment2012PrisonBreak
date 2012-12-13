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

			GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;

			RigidBody.DebugLoadContent(GraphicsDevice, Content);
			DialogueRenderer.Instance.Initialize(Content, GraphicsDevice);

			manager = new GameObjectManager();

			GameObject camera = new GameObject();
			camera.AddTransform();
			//camera.Transform.Parent = player.Transform;
			camera.Transform.Position = new Vector2(0f, 30f);
			camera.Transform.Z = 80f;
			camera.AddCamera(GraphicsDevice.Viewport, true);
			camera.AddScript(new CameraScript(camera));
			manager.AddGameObject(camera);

			GameObject splash = SplashScript.CreateSplashGO(Content, GraphicsDevice);
			splash.Transform.Translate(new Vector3(-3000f, 0f, 10));
			manager.AddGameObject(splash);

			//GameObject cameraBounds = GameObject.CreateStaticGO(GraphicsDevice, Content.Load<Texture2D>("DebugCameraBounds"), SpriteTransparency.Transparent);
			//cameraBounds.Transform.Z = 0f;
			//cameraBounds.Transform.Parent = camera.Transform;
			//manager.AddGameObject(cameraBounds);

			// Main area
			GameObject mainLevel = WorldGen.CreateWorldGO(GraphicsDevice, Content, "Levels/LevelMainMid", "Levels/LevelMain");
			mainLevel.RigidBody.Body.Friction = 4f;
			manager.AddGameObject(mainLevel);
			GameObject mainLevelBack = GameObject.CreateStaticGO(GraphicsDevice, Content.Load<Texture2D>("Levels/LevelMainBack"), SpriteTransparency.Opaque);
			mainLevelBack.Transform.Z -= 25f;
			manager.AddGameObject(mainLevelBack);
			GameObject mainLevelFront = GameObject.CreateStaticGO(GraphicsDevice, Content.Load<Texture2D>("Levels/LevelMainFront"), SpriteTransparency.Transparent);
			mainLevelFront.Transform.Z = 5f;
			manager.AddGameObject(mainLevelFront);

			// Vent area
			GameObject vents = WorldGen.CreateWorldGO(GraphicsDevice, Content, "Levels/VentsMid", "Levels/Vents");
			vents.RigidBody.Body.Friction = 0f;
			vents.Transform.Position = new Vector2(2000f, 0f);
			manager.AddGameObject(vents);
			GameObject ventsTop1 = GameObject.CreateStaticGO(GraphicsDevice, Content.Load<Texture2D>("Levels/VentsTop1"), SpriteTransparency.Transparent);
			ventsTop1.Transform.Translate(new Vector3(2000f, 0f, 40f));
			manager.AddGameObject(ventsTop1);
			GameObject ventsTop2 = GameObject.CreateStaticGO(GraphicsDevice, Content.Load<Texture2D>("Levels/VentsTop2"), SpriteTransparency.Transparent);
			ventsTop2.Transform.Translate(new Vector3(2000f, 0f, 30f));
			manager.AddGameObject(ventsTop2);

			// Fans
            GameObject fan1 = FanScript.CreateFanGO(Content, GraphicsDevice);
            manager.AddGameObject(fan1);
            fan1.Transform.Translate(new Vector3(1855f, 180f, 0f));
			GameObject fan2 = FanScript.CreateFanGO(Content, GraphicsDevice);
			manager.AddGameObject(fan2);
			fan2.Transform.Translate(new Vector3(2272f, 256f, 0f));
			GameObject fan3 = FanScript.CreateFanGO(Content, GraphicsDevice);
			manager.AddGameObject(fan3);
			fan3.Transform.Translate(new Vector3(2016f, -208f, 0f));

			GameObject player = PlayerScript.CreatePlayerGO(Content, GraphicsDevice);
			//player.Transform.Translate(new Vector3(-200f, -40f, 1f));
			player.Transform.Translate(new Vector3(2385f, -420f, 1f));
			manager.AddGameObject(player);
			camera.Transform.Parent = player.Transform;

			PlayerScript playerScript = player.GetComponent<PlayerScript>();
			GameObject teleporter1 = TeleporterScript.CreateTeleporterGO(Content, playerScript, new Vector2(1604f, -332f), true);
			teleporter1.Transform.Translate(new Vector2(250f, -230f));
			manager.AddGameObject(teleporter1);
			GameObject teleporter2 = TeleporterScript.CreateTeleporterGO(Content, playerScript, new Vector2(-400f, 140f), false);
			teleporter2.Transform.Translate(new Vector2(1746f, 396f));
			manager.AddGameObject(teleporter2);
			GameObject teleporter3 = TeleporterScript.CreateTeleporterGO(Content, playerScript, new Vector2(2140f, 120f), true);
			teleporter3.Transform.Translate(new Vector2(330f, 140f));
			manager.AddGameObject(teleporter3);
			GameObject teleporter4 = TeleporterScript.CreateTeleporterGO(Content, playerScript, new Vector2(600f, 140f), false);
			teleporter4.Transform.Translate(new Vector2(2450f, 340f));
			manager.AddGameObject(teleporter4);
			GameObject teleporter5 = TeleporterScript.CreateTeleporterGO(Content, playerScript, new Vector2(1604f, -332f), true);
			teleporter5.Transform.Translate(new Vector2(800f, 140f));
			manager.AddGameObject(teleporter5);
			GameObject teleporter6 = TeleporterScript.CreateTeleporterGO(Content, playerScript, new Vector2(600f, -230f), false);
			teleporter6.Transform.Translate(new Vector2(2385f, -420f));
			manager.AddGameObject(teleporter6);

			GameObject tyson = ShankTargetScript.CreateTysonGO(Content, GraphicsDevice, playerScript);
			tyson.Transform.Translate(new Vector2(775f, 180f));
			manager.AddGameObject(tyson);

			//GameObject laser = LaserScript.CreateLaserGO(Content, GraphicsDevice);
			//manager.AddGameObject(laser);
			//laser.Transform.Translate(new Vector3(400f, 30f, 0f));

			//List<Vector2> patrolPoints = GuardScript.CreatePatrolPoints(200f, -50f, 300f, -50f, 400f, -50f, 500f, -50f);
			//GameObject guard = GuardScript.CreateGuardGO(Content, GraphicsDevice, patrolPoints);
			//guard.Transform.Translate(new Vector2(-300f, -50f));
			//manager.AddGameObject(guard);

			GameObject shank = ShankScript.CreateShankGO(Content, GraphicsDevice, (PlayerScript)player.GetComponent<PlayerScript>());
			manager.AddGameObject(shank);

			GameObject superStar = new GameObject();
			superStar.AddTransform();
			superStar.Transform.Translate(new Vector2(692f, -200f));
			superStar.AddAnimation(Content.Load<Texture2D>("Pickups/Superstar"), new Vector2(14f, 20f));
			superStar.Animation.AddAnimation("Idle", 0, 4);
			superStar.Animation.Play("Idle");
			superStar.AddRenderer(GraphicsDevice, SpriteTransparency.Transparent);
			manager.AddGameObject(superStar);

			List<string> kkDi1 = new List<string>(11);
			kkDi1.Add("Who's out there?");
			kkDi1.Add("Just a man looking for his place in this cesspool.");
			kkDi1.Add("Well, this ain't no zoo so unless you gots anything for me then scram.");
			kkDi1.Add("What could an oversized turtle want in prison?");
			kkDi1.Add("Turtle!? When I have my koopas catch yo -");
			kkDi1.Add("You're minions couldn't catch a fat 3 foot tall plumber, even if they had wings.");
			kkDi1.Add("If my flying koopas in here hadn't had their wings pulled off by that damn Donkey Kong I'd have you before getting out of this cell.");
			kkDi1.Add("Ripping their wings off? Why'd he do that?");
			kkDi1.Add("That damn monkey was trying to muscle in on my woman. After Pauline disappeared he went after my princess before coming here. I put a hit on him but the damn monkey's too tough for my troopas. But when I'm out of this cell not only is he a dead chimp but I'll be busting out of here real soon.");
			kkDi1.Add("How do you intend on escaping the prison?");
			kkDi1.Add("None of your business. But if you bring me that primates head I might let you in on the plan.");
			List<string> kkCh1 = new List<string>(11);
			kkCh1.Add("Bowser");
			kkCh1.Add("Player");
			kkCh1.Add("Bowser");
			kkCh1.Add("Player");
			kkCh1.Add("Bowser");
			kkCh1.Add("Player");
			kkCh1.Add("Bowser");
			kkCh1.Add("Player");
			kkCh1.Add("Bowser");
			kkCh1.Add("Player");
			kkCh1.Add("Bowser");
			GameObject kkDGO1 = DialogueBoxScript.CreateDialogueAreaGO(kkDi1, kkCh1, new Vector2(96f, 64f));
			kkDGO1.Transform.Position = new Vector2(-415f, -220f);
			manager.AddGameObject(kkDGO1);

			List<string> gDi1 = new List<string>(11);
			gDi1.Add("Who dares to disturb me?");
			gDi1.Add("What exactly am I disturbing you from? You're alone in an empty cell.");
			gDi1.Add("Insolence! Once I escape from this cell I'll get you and Mike Tyson.");
			gDi1.Add("Mike Tyson? What's he got to do with you?");
			gDi1.Add("That Neanderthal has been trying to eat my ear.");
			gDi1.Add("Typical.");
			gDi1.Add("Once he's out of the way I'll be making my escape from this abysmal pit.");
			gDi1.Add("You know of a way to get out of this place?");
			gDi1.Add("Indeed. Once Mike Tyson is out of my way I'll make my glorious return to Hyrule.");
			gDi1.Add("How do you intend on escaping?");
			gDi1.Add("Lowly creature! If you do not have anything to offer me then why would I tell you? Kill Mike Tyson and I may divulge my plans to you.");
			List<string> gCh1 = new List<string>(11);
			gCh1.Add("Ganon");
			gCh1.Add("Player");
			gCh1.Add("Ganon");
			gCh1.Add("Player");
			gCh1.Add("Ganon");
			gCh1.Add("Player");
			gCh1.Add("Ganon");
			gCh1.Add("Player");
			gCh1.Add("Ganon");
			gCh1.Add("Player");
			gCh1.Add("Ganon");
			GameObject gDGO1 = DialogueBoxScript.CreateDialogueAreaGO(gDi1, gCh1, new Vector2(96f, 64f));
			gDGO1.Transform.Position = new Vector2(-190f, -220f);
			manager.AddGameObject(gDGO1);

			List<string> mbDi1 = new List<string>(14);
			mbDi1.Add("What the hell am I looking at?");
			mbDi1.Add("This one may be useful to me.");
			mbDi1.Add("I'm hearing voices in my head. That plumber hit me harder than I thought.");
			mbDi1.Add("I am Mother Brain, leader of the space pirates.");
			mbDi1.Add("I'm not really comfortable with people being in my head.");
			mbDi1.Add("My apologies, I do not have vocal cords or orifice to amplify sound vibrations in or-");
			mbDi1.Add("Please stop talking, it's hurting my brain.");
			mbDi1.Add("I only wish to make a proposition.");
			mbDi1.Add("Go on then?");
			mbDi1.Add("There is a man by the name of Doctor Wily who has challenged my pirates against his robot masters. I only seek his departure from this universe.");
			mbDi1.Add("You want him dead? What's in it for me?");
			mbDi1.Add("I can offer you freedom.");
			mbDi1.Add("How?");
			mbDi1.Add("Once this task is complete I will reveal the means of escape.");
			List<string> mbCh1 = new List<string>(14);
			mbCh1.Add("Player");
			mbCh1.Add("MotherBrain");
			mbCh1.Add("Player");
			mbCh1.Add("MotherBrain");
			mbCh1.Add("Player");
			mbCh1.Add("MotherBrain");
			mbCh1.Add("Player");
			mbCh1.Add("MotherBrain");
			mbCh1.Add("Player");
			mbCh1.Add("MotherBrain");
			mbCh1.Add("Player");
			mbCh1.Add("MotherBrain");
			mbCh1.Add("Player");
			mbCh1.Add("MotherBrain");
			GameObject mbDGO1 = DialogueBoxScript.CreateDialogueAreaGO(mbDi1, mbCh1, new Vector2(96f, 64f));
			mbDGO1.Transform.Position = new Vector2(45f, -220f);
			manager.AddGameObject(mbDGO1);

			List<string> dkDi = new List<string>(7);
			dkDi.Add("oooOOOAAHH!");
			dkDi.Add("Got you now Kong.");
			dkDi.Add("What is the meaning of this?");
			dkDi.Add("The monkey can talk?");
			dkDi.Add("Sir, this is an outrageous breach of my personal space. And, I'll have you know that I am of the Hominoidea family, therefore, I am an ape, not a monkey.");
			dkDi.Add("King Koopa sends his regards.");
			dkDi.Add("“Have at thee.");
			List<string> dkCh = new List<string>(7);
			dkCh.Add("DK");
			dkCh.Add("Player");
			dkCh.Add("DK");
			dkCh.Add("Player");
			dkCh.Add("DK");
			dkCh.Add("Player");
			dkCh.Add("DK");
			GameObject dkDGO = DialogueBoxScript.CreateDialogueAreaGO(dkDi, dkCh, new Vector2(240f, 112f));
			dkDGO.Transform.Position = new Vector2(700f, 170f);
			dkDGO.Trigger.Enabled = false;
			manager.AddGameObject(dkDGO);

			List<string> mtDi = new List<string>(5);
			mtDi.Add("Hey, what's going on here?");
			mtDi.Add("Your time has come champ.");
			mtDi.Add("Quit trying to scrutinize with my brain. I'm going to kick your ass.");
			mtDi.Add("You couldn't beat that punk ass Little Mac who was half your size.");
			mtDi.Add("I'ma eat your children son.");
			List<string> mtCh = new List<string>(5);
			mtCh.Add("Tyson");
			mtCh.Add("Player");
			mtCh.Add("Tyson");
			mtCh.Add("Player");
			mtCh.Add("Tyson");
			GameObject mtDGO = DialogueBoxScript.CreateDialogueAreaGO(mtDi, mtCh, new Vector2(50f, 112f));
			mtDGO.Transform.Position = new Vector2(620f, 170f);
			manager.AddGameObject(mtDGO);

			List<string> wDi = new List<string>(4);
			wDi.Add("What's going on here?");
			wDi.Add("Dr. Wily, your license's been revoked.");
			wDi.Add("License? I'm not a medical doctor.");
			wDi.Add("Shut it and die egghead.");
			List<string> wCh = new List<string>();
			wCh.Add("Wily");
			wCh.Add("Player");
			wCh.Add("Wily");
			wCh.Add("Player");
			GameObject wDGO = DialogueBoxScript.CreateDialogueAreaGO(wDi, wCh, new Vector2(240f, 112f));
			wDGO.Transform.Position = new Vector2(700f, 170f);
			wDGO.Trigger.Enabled = false;
			manager.AddGameObject(wDGO);
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
