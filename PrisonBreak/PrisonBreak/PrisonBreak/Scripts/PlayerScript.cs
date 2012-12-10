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
		float moveSpeed = 40f;
		float maxSpeed = 150f;

		public PlayerScript(GameObject parent)
			: base(parent)
		{
			Animation.Play("idle");
		}

		public override void Update()
		{
			Vector2 movement = Vector2.Zero;
			if (Input.KeyboardState.IsKeyDown(Keys.W))
			{
				movement.Y += 1f;
			}
			if (Input.KeyboardState.IsKeyDown(Keys.S))
			{
				movement.Y -= 1f;
			}
			if (Input.KeyboardState.IsKeyDown(Keys.A))
			{
				movement.X -= 1f;
				Renderer.IsFlipped = true;
			}
			if (Input.KeyboardState.IsKeyDown(Keys.D))
			{
				movement.X += 1f;
				Renderer.IsFlipped = false;
			}

			if (movement.Length() > 0)
			{
				movement.Normalize();
				movement /= RigidBody.MInPx;
				if (RigidBody.Body.LinearVelocity.LengthSquared() < maxSpeed)
				RigidBody.ApplyImpulse(movement * moveSpeed);
				Animation.Play("run");
			}
			else
			{
				Animation.Play("idle");
			}
		}
	}
}

