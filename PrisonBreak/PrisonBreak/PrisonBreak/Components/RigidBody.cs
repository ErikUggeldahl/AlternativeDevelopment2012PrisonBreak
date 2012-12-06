using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using FarseerPhysics;
using FarseerPhysics.DebugViews;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

namespace PrisonBreak.Components
{
	public class CollisionCats
	{
		public static Category PlayerCategory { get { return Category.Cat2; } }
		public static Category WorldCategory { get { return Category.Cat3; } }
		public static Category EnemyCategory { get { return Category.Cat4; } }
		public static Category TriggerCategory { get { return Category.Cat5; } }
	}

	public class RigidBody : BaseComponent
	{
		private static World world = new World(new Vector2(0f, -9.82f));
		private static DebugViewXNA debugView = new DebugViewXNA(world);
		public const float MInPx = 33f;	// TODO: Change MInPx to account for camera Z

		public static World World
		{
			get { return world; }
		}

		private Body body;

		public Body Body
		{
			get { return body; }
		}

		public Category CollidesWith
		{
			set { body.CollidesWith = value; }
		}

		public Category CollisionCategory
		{
			set { body.CollisionCategories = value; }
		}

		static RigidBody()
		{
			debugView.AppendFlags(DebugViewFlags.DebugPanel);
			debugView.DefaultShapeColor = Color.White;
			debugView.SleepingShapeColor = Color.LightGray;
		}

		public static void DebugLoadContent(GraphicsDevice graphicsDevice, ContentManager content)
		{
			debugView.LoadContent(graphicsDevice, content);
		}

		public static void DebugRender()
		{
			/*Matrix projection = Matrix.CreateOrthographicOffCenter(0f, Camera.MainCamera.Viewport.Width / MInPx, Camera.MainCamera.Viewport.Height / MInPx, 0f, 0f, 1f);
			Matrix view = Matrix.CreateTranslation(new Vector3(-Camera.MainCamera.Transform.Position / MInPx, 0f)) *
				Matrix.CreateTranslation(new Vector3(-Camera.MainCamera.Origin / MInPx, 0)) *
				Matrix.CreateRotationZ(Camera.MainCamera.Transform.Rotation) *
				Matrix.CreateScale(1f) *
                Matrix.CreateTranslation(new Vector3(Camera.MainCamera.Transform.Position / MInPx, 0f));
			debugView.RenderDebugData(ref projection, ref view);*/
		}

		public RigidBody(GameObject parent, BodyType bodyType, Vector2 size)
			: base(parent)
		{
			body = BodyFactory.CreateRectangle(world, size.X / MInPx, size.Y / MInPx, 1f, Transform.Position / MInPx);
			body.BodyType = bodyType;
			body.Friction = 2f;
			body.FixedRotation = true;
		}

		public override void Update()
		{
			if (body.BodyType == BodyType.Static)
				return;
			Transform.Position = body.Position * MInPx;
			Transform.RotationZ = body.Rotation;
		}

		public void ApplyImpulse(Vector2 impulse)
		{
			body.ApplyLinearImpulse(impulse);
		}

		public static void WorldStep()
		{
			world.Step(GameTimeGlobal.DeltaSec);
		}
	}
}
