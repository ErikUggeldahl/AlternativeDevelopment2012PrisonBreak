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
        float moveSpeed = 20f;
        float maxSpeed = 100f;

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
                go.Animation.Play("run");
            }
            if (Input.KeyboardState.IsKeyDown(Keys.S))
            {
                movement.Y += 1f;
                go.Animation.Play("run");
            }
            if (Input.KeyboardState.IsKeyDown(Keys.A))
            {
                movement.X -= 1f;
                go.Animation.Play("run");
                go.Renderer.IsFlipped = true;
                
            }
            if (Input.KeyboardState.IsKeyDown(Keys.D))
            {
                movement.X += 1f;
                go.Animation.Play("run");
                go.Renderer.IsFlipped = false;
               
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
            if (Input.KeyboardState.IsKeyDown(Keys.Q))
            {
                movement.X += 1f;
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
            else
            {
                go.Animation.Play("idle");
            }

            if (Input.GamepadState.IsConnected)
            {
                if (Input.GamepadState.IsButtonDown(Buttons.LeftThumbstickRight))
                {
                    go.Animation.Play("run");
                    go.Transform.Translate(new Vector2(2f, 0f));
                }
                if (Input.GamepadState.IsButtonDown(Buttons.LeftThumbstickLeft))
                {
                    go.Transform.Translate(new Vector2(-2f, 0f));
                    go.Animation.Play("run");
                }
                if (Input.GamepadState.IsButtonDown(Buttons.LeftThumbstickUp))
                {
                    go.Transform.Translate(new Vector2(0f, -2f));
                    go.Animation.Play("run");
                }
                if (Input.GamepadState.IsButtonDown(Buttons.LeftThumbstickDown))
                {
                    go.Transform.Translate(new Vector2(0f, 2f));
                    go.Animation.Play("run");
                }
                if (Input.GamepadState.IsButtonDown(Buttons.A))
                {
                    go.Transform.Translate(new Vector2(0f, 2f));
                    go.Animation.Play("stealth");
                }
                if (Input.GamepadState.IsButtonDown(Buttons.X))
                {
                    go.Animation.Play("stab");
                }
                if (Input.GamepadState.IsButtonDown(Buttons.Y))
                {
                    go.Transform.Translate(new Vector2(2f, 0f));
                    go.Animation.Play("climb");
                }

                //the PLayer's special move
                //if (Input.GamepadState.IsButtonDown(Buttons.A))
                //{

                //}

            }
        }
    }
}


