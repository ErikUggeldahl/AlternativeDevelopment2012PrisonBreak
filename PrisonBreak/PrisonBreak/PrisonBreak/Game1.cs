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
        /// 

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            RigidBody.DebugLoadContent(GraphicsDevice, Content);

            manager = new GameObjectManager();

            GameObject player = new GameObject();
            player.AddTransform();
            player.Transform.Translate(new Vector2(30f, 620f));
           
            player.AddAnimation(Content.Load<Texture2D>("MainCharacter"), new Rectangle(0, 0, 30, 30));
            player.Animation.AddAnimation("idle", 0, 1);
            player.Animation.AddAnimation("run", 0, 4);
            player.Animation.AddAnimation("stab", 2, 1);
            player.Animation.AddAnimation("stealth", 1, 4);
            player.Animation.AddAnimation("climb", 3, 4);
            player.AddRenderer(spriteBatch);
            player.Animation.Play("idle");
            player.AddDynamicRigidBody(new Vector2(30f, 30f));

            manager.AddGameObject(player);

            //List<Vector2> patrolPoints = GuardScript.CreatePatrolPoints(200f, 620f, 300f, 620f, 400f, 620f, 500f, 620f, 500f, 620f);

            //GameObject guard = GuardScript.CreateGuardGO(spriteBatch, Content, patrolPoints);
            //guard.Transform.Translate(new Vector2(0f, 610f));
            //manager.AddGameObject(guard);

            //patrolPoints = GuardScript.CreatePatrolPoints(900f, 620f, 800f, 620f, 700f, 620f, 600f, 620f, 540f, 620f);

            //GameObject guard2 = GuardScript.CreateGuardGO(spriteBatch, Content, patrolPoints);
            //guard2.Transform.Translate(new Vector2(1000f, 610f));
            //manager.AddGameObject(guard2);

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
