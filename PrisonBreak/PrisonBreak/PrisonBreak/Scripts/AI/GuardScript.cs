using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



using Microsoft.Xna.Framework;
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

        float moveSpeed = 1f;
        float maxSpeed = 10f;
        float tolerance = 10f;
        TimeSpan MoveTime = TimeSpan.FromSeconds(5.0f);
        TimeSpan TimePassed = TimeSpan.Zero;

        int currentTargetCount = 0;

        //Vector2 targetA;
        //Vector2 targetB;
        //Vector2 targetC;
        //Vector2 targetD;
        //Vector2 targetE;


        Vector2 currentTarget;

        TimeSpan MaxWaitTime = TimeSpan.FromSeconds(1);
        TimeSpan WaitTimeCounter = TimeSpan.Zero;


        //public void AddVectors(Vector2 point)
        //{
        //    targetA = new Vector2(200f, 620f);
        //    targetB = new Vector2(600f, 620f);
        //    targetC = new Vector2(650f, 620f);
        //    targetD = new Vector2(540f, 620f);
        //    targetE = new Vector2(300f, 620f);

        //    points.Add(targetA);
        //    points.Add(targetB);
        //    points.Add(targetC);
        //    points.Add(targetD);
        //    points.Add(targetE);
        //}
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
                    TimePassed = TimeSpan.Zero;
                    currentTarget = points[currentTargetCount];
                    currentTargetCount++;
                    if (currentTargetCount >= 5) 
                    currentTargetCount = 0;
                    WaitTimeCounter = TimeSpan.Zero;
                    
                   
                }
                //else if (currentTarget == points[currentTargetCount])
                //{
                //    currentTarget = points[currentTargetCount];
              
                //        go.Renderer.IsFlipped = true;
                    
                //}

            }
        }

        public override void Update()
        {

            DirectionMoving();

            //if (TimePassed < MoveTime)
            //{

            //    TimePassed += GameTimeGlobal.GameTime.ElapsedGameTime;

            //    if (TimePassed >= MoveTime)
            //    {
            //        TimePassed = MoveTime;
            //    }


                //float LerpAmount = (float)TimePassed.Ticks / MoveTime.Ticks;
                //currentPosition = Vector2.Lerp(source, destination, LerpAmount);
                //go.Transform.Position = currentPosition;


            //}
            //else
            //{

            //    WaitTimeCounter += GameTimeGlobal.GameTime.ElapsedGameTime;
            //    if (WaitTimeCounter > MaxWaitTime)
            //    {
                    //TimePassed = TimeSpan.Zero;
                    //destination = source;
                    //source = currentPosition;
                    //WaitTimeCounter = TimeSpan.Zero;
                //}



            //}
        }



    }
}
