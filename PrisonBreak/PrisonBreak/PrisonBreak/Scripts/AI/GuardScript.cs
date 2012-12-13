using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using PrisonBreak.Components;

namespace PrisonBreak.Scripts.AI
{
    public class GuardScript : Script
    {
		private List<Vector2> points;

		private float moveSpeed = 15f;
		private float maxSpeed = 30f;
		private float tolerance = 50f;

		private int currentTargetCount = 0;
		private Vector2 currentTarget;

        private TimeSpan MaxWaitTime = TimeSpan.FromSeconds(1);
		private TimeSpan WaitTimeCounter = TimeSpan.Zero;

		private static Texture2D guardTexture;

        public GuardScript(GameObject parent, List<Vector2> points)
            : base(parent)
        {
            this.points = points;

            currentTarget = points[currentTargetCount];
        }

        public override void Update()
        {
			Vector2 direction = currentTarget - Transform.Position;
			direction = new Vector2(direction.X, 0f);

			if ((Transform.WorldPosition2 - currentTarget).Length() > tolerance)
			{
				if (direction.X < 0)
					Renderer.IsFlipped = true;
				else
					Renderer.IsFlipped = false;

				direction.Normalize();
				direction *= moveSpeed;

				if (RigidBody.Body.LinearVelocity.LengthSquared() <= maxSpeed)
					RigidBody.ApplyImpulse(direction);

				Animation.Play("Run");
			}
			else
			{
				Animation.Play("Idle");

				WaitTimeCounter += GameTimeGlobal.GameTime.ElapsedGameTime;
				if (WaitTimeCounter > MaxWaitTime)
				{
					currentTarget = points[currentTargetCount];
					currentTargetCount++;

					if (currentTargetCount >= points.Count)
						currentTargetCount = 0;

					WaitTimeCounter = TimeSpan.Zero;
				}
			}
        }

        public static GameObject CreateGuardGO(ContentManager content, GraphicsDevice gd, List<Vector2> points)
        {
            if (guardTexture == null)
            {
                guardTexture = content.Load<Texture2D>("Guard");
            }

            GameObject guard = new GameObject();
            
            guard.AddTransform();
            guard.AddAnimation(guardTexture, new Vector2(20f, 34f));
            guard.AddScript(new GuardScript(guard, points));
            guard.Animation.AddAnimation("Idle", 1, 1);
            guard.Animation.AddAnimation("Run", 0, 4);
            guard.AddDynamicRigidBody(new Vector2(20f, 34f));
            guard.Animation.Play("Idle");
            guard.AddRenderer(gd, SpriteTransparency.Transparent);

            return guard;
        }
        public static GameObject CreateGuardDogGO(ContentManager content, GraphicsDevice gd, List<Vector2> points)
        {
            if (guardTexture == null)
            {
                guardTexture = content.Load<Texture2D>("Characters/DuckHuntDog");
            }

            GameObject guard = new GameObject();

            guard.AddTransform();
            guard.AddAnimation(guardTexture, new Vector2(58f, 50.5f));
            guard.AddScript(new GuardScript(guard, points));
            guard.Animation.AddAnimation("Idle", 1, 1);
            guard.Animation.AddAnimation("Run", 0, 4);
            guard.AddDynamicRigidBody(new Vector2(58f, 50.5f));
            guard.Animation.Play("Idle");
            guard.AddRenderer(gd, SpriteTransparency.Transparent);

            return guard;
        }
        public static List<Vector2> CreatePatrolPoints(params float[] points)
        {
            List<Vector2> patrolPoints = new List<Vector2>();

            if (points.Length % 2 != 0)
                throw new ArgumentException("Uneven number of patrol points.");

            for (int i = 0; i < points.Length; i += 2)
            {
                patrolPoints.Add(new Vector2(points[i], points[i + 1]));
            }

            return patrolPoints;
        }
    }
}
