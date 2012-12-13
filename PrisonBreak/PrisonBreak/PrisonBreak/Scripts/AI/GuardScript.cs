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
        //Vector2 source = Vector2.Zero;
        //Vector2 destination = new Vector2(1000, 0);
        //Vector2 currentPosition;
        List<Vector2> points;

        float moveSpeed = 0.5f;
        float maxSpeed = 10f;
        float tolerance = 10f;
        TimeSpan MoveTime = TimeSpan.FromSeconds(5.0f);
        TimeSpan TimePassed = TimeSpan.Zero;

        int currentTargetCount = 0;

        Vector2 currentTarget;

        TimeSpan MaxWaitTime = TimeSpan.FromSeconds(1);
        TimeSpan WaitTimeCounter = TimeSpan.Zero;

        public GuardScript(GameObject parent, List<Vector2> points)
            : base(parent)
        {
            this.points = points;

            currentTarget = points[currentTargetCount];
        }



        public void DirectionMoving()
        {

            Vector2 direction = currentTarget - go.Transform.Position;
            direction = new Vector2(direction.X, 0f);

            if ((currentTarget - (go.Transform.Position + go.RigidBody.Body.LinearVelocity)).Length() > tolerance)
            {

                direction.Normalize();
                if (direction.X < 0)
                {
                    go.Renderer.IsFlipped = true;
                }
                else
                    go.Renderer.IsFlipped = false;
                direction *= moveSpeed;
                if (go.RigidBody.Body.LinearVelocity.LengthSquared() <= maxSpeed)
                    go.RigidBody.ApplyImpulse(direction);

                go.Animation.Play("run");
            }
            else
            {
                go.Animation.Play("idle");

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

        public override void Update()
        {

            DirectionMoving();

        }

        private static Texture2D guardTexture;
        private static SpriteBatch spriteBatch;



        public static GameObject CreateGuardGO(SpriteBatch sb, ContentManager content, List<Vector2> points)
        {
            if (guardTexture == null)
            {
                guardTexture = content.Load<Texture2D>("Guard");
            }
            if (sb == null)
            {
                sb = spriteBatch;
            }



            GameObject guard = new GameObject();

            guard.AddTransform();
            //guard.Transform.Translate( new Vector2(30f, 620f));
            guard.AddAnimation(guardTexture, new Rectangle(0, 0, 20, 34));
            guard.AddScript(new GuardScript(guard, points));
            guard.Animation.AddAnimation("idle", 1, 1);
            guard.Animation.AddAnimation("run", 0, 4);
            guard.AddDynamicRigidBody(new Vector2(20f, 34f));
            guard.Animation.Play("idle");
            guard.AddRenderer(sb);

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
