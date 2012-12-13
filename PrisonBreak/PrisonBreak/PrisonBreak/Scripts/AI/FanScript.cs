using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using PrisonBreak.Components;

namespace PrisonBreak.Scripts.AI
{
	public class FanScript : Script
	{
        float moveSpeed = 5f;
        float maxSpeed = 10f;

        float tolerance = 10f;

        TimeSpan MaxWaitTime = TimeSpan.FromSeconds(1.0f);
        TimeSpan WaitTimeCounter = TimeSpan.Zero;

        Vector2 currentPos;
        Vector2 finalPos;

		static Texture2D fanSprite;

        List<Vector2> points;

		public FanScript(GameObject parent)
			: base(parent)
		{
		}

		private bool TriggerEnter(FarseerPhysics.Dynamics.Fixture fixtureA, FarseerPhysics.Dynamics.Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
		{
			return true;
		}

		public override void Update()
		{

            currentPos = new Vector2(100f, 0f);
            finalPos = new Vector2(200f, 0f);

            Vector2 direction = currentPos - go.Transform.Position;
            direction = new Vector2(direction.X, 0f);

            if ((currentPos - (go.Transform.Position + go.RigidBody.Body.LinearVelocity)).Length() > tolerance)
            {
                direction.Normalize();
                direction *= moveSpeed;

                if (go.RigidBody.Body.LinearVelocity.LengthSquared() <= maxSpeed)
                    go.RigidBody.ApplyImpulse(direction);
            }
            else
            {
                go.Animation.Play("Fan");

                WaitTimeCounter += GameTimeGlobal.GameTime.ElapsedGameTime;
                if (WaitTimeCounter > MaxWaitTime)
                {
                    MaxWaitTime = TimeSpan.Zero;
                    currentPos = finalPos;
                    
                }
            }
		}

		public static GameObject CreateFanGO(ContentManager manager, GraphicsDevice gd)
		{
			if (fanSprite == null)
			{
				fanSprite = manager.Load<Texture2D>("Fan");
			}

			GameObject fan = new GameObject();

			fan.AddTransform();
			fan.AddAnimation(fanSprite, new Vector2(62f, 58f));
			fan.Animation.AddAnimation("Spin", 0, 2);
			fan.Animation.Play("Spin");
            fan.AddDynamicRigidBody(new Vector2(62f, 58f));
			fan.AddRenderer(gd, SpriteTransparency.Opaque);
			fan.AddTrigger(new Vector2(62f, 58f));
			fan.Trigger.CollidesWith = CollisionCats.PlayerCategory;
			FanScript fanScript = new FanScript(fan);
			fan.AddScript(fanScript);
			fan.Trigger.OnEnter += new FarseerPhysics.Dynamics.OnCollisionEventHandler(fanScript.TriggerEnter);

			return fan;
		}

        public static List<Vector2> CreatePoint(params float[] points)
        {
            List<Vector2> movePoints = new List<Vector2>();
            if (points.Length % 2 != 0)
            throw new ArgumentException("Uneaven NumberOf Move Points");

            for (int i = 0; i < points.Length; i += 2)
            {
                movePoints.Add(new Vector2(points[i], points[i + 1]));
            }
            return movePoints;
            
        }

	}
}
