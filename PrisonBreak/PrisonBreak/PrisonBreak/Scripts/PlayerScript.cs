using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using PrisonBreak.Components;

namespace PrisonBreak.Scripts
{
    public class PlayerScript : Script
    {
        float moveSpeed = 12f;
        float maxSpeed = 20f;

        public PlayerScript(GameObject parent)
            : base(parent)
        {
            go.Animation.Play("idle");
        }

        public override void Update()
        {
            Vector2 movement = Vector2.Zero;
            if (Input.KeyboardState.IsKeyDown(Keys.W))
            {
                movement.Y -= 1f;
                go.Animation.Play("climb");
            }
            if (Input.KeyboardState.IsKeyDown(Keys.S))
            {
                movement.Y += 1f;
                go.Animation.Play("climb");
            }
            if (Input.KeyboardState.IsKeyDown(Keys.A))
            {
                movement.X -= 1f;
                go.Animation.Play("run");
                go.Renderer.IsFlipped = false;

            }
            if (Input.KeyboardState.IsKeyDown(Keys.D))
            {
                movement.X += 1f;
                go.Animation.Play("run");
                go.Renderer.IsFlipped = true;

            }
            if (Input.KeyboardState.IsKeyDown(Keys.Q))
            {
                go.Animation.Play("stab");
            }
            if (Input.KeyboardState.IsKeyDown(Keys.E))
            {
                movement.X += 1f;
                go.Animation.Play("stealth");
            }

            if (movement.Length() > 0)
            {
                movement.Normalize();
                movement /= RigidBody.MInPx;
                if (go.RigidBody.Body.LinearVelocity.LengthSquared() < maxSpeed)
                    go.RigidBody.ApplyImpulse(movement * moveSpeed);
                go.Animation.Play("run");
            }
            else
            {
                go.Animation.Play("idle");
            }

            if (Input.GamepadState.IsConnected)
            {
                
                if (Input.GamepadState.ThumbSticks.Left.X > 0 )// .IsButtonDown(Buttons.LeftThumbstickRight))
                {
                    movement.X += 1f;
                    go.Renderer.IsFlipped = true;
                    go.Animation.Play("run");

                }
                if (Input.GamepadState.ThumbSticks.Left.X < 0)//if (Input.GamepadState.IsButtonDown(Buttons.LeftThumbstickLeft))
                {
                    movement.X -= 1f;
                    go.Renderer.IsFlipped = false;
                    go.Animation.Play("run");
                }
                if (Input.GamepadState.ThumbSticks.Left.Y > 0)//if (Input.GamepadState.IsButtonDown(Buttons.LeftThumbstickUp))
                {
                    movement.Y -= 1f;
                    go.Animation.Play("climb");
                }
                if (Input.GamepadState.ThumbSticks.Left.Y < 0)// if (Input.GamepadState.IsButtonDown(Buttons.LeftThumbstickDown))
                {
                    movement.Y += 1f;
                    go.Animation.Play("climb");
                }
                if (Input.GamepadState.IsButtonDown(Buttons.A))
                {
                    go.Animation.Play("stealth");
                }

                if (Input.GamepadState.IsButtonDown(Buttons.X))
                {
                    go.Animation.Play("stab");
                }

                if (Input.GamepadState.IsButtonDown(Buttons.Y))
                {
                    movement.Y += 1f;
                    go.Animation.Play("climb");
                }

                if (movement.Length() > 0)
                {
                    movement.Normalize();
                    movement /= RigidBody.MInPx;
                    if (go.RigidBody.Body.LinearVelocity.LengthSquared() < maxSpeed)
                        go.RigidBody.ApplyImpulse(movement * moveSpeed);
                    go.Animation.Play("run");
                }
                //else
                //{
                //    go.Animation.Play("idle");
                //}
            }
        }
    }
}


