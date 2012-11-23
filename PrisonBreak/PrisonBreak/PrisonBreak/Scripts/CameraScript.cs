using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using PrisonBreak.Components;

namespace PrisonBreak.Scripts
{
	public class CameraScript : Script
	{
		float moveSpeed = 200f;

		public CameraScript(GameObject parent)
			: base(parent)
		{
		}

		public override void Update()
		{
			Vector2 movement = Vector2.Zero;
			if (Input.KeyboardState.IsKeyDown(Keys.Up))
			{
				movement.Y -= 1f;
			}
			if (Input.KeyboardState.IsKeyDown(Keys.Down))
			{
				movement.Y += 1f;
			}
			if (Input.KeyboardState.IsKeyDown(Keys.Left))
			{
				movement.X -= 1f;
			}
			if (Input.KeyboardState.IsKeyDown(Keys.Right))
			{
				movement.X += 1f;
			}

			if (Input.KeyboardState.IsKeyDown(Keys.Q))
			{
				Transform.Z += 100f * GameTimeGlobal.DeltaSec;
			}
			if (Input.KeyboardState.IsKeyDown(Keys.E))
			{
				Transform.Z -= 100f * GameTimeGlobal.DeltaSec;
			}

			if (movement.Length() > 0)
			{
				movement.Normalize();
				Transform.Translate(movement * moveSpeed * GameTimeGlobal.DeltaSec);
			}
		}
	}
}
