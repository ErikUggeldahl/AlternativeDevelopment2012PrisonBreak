using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
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
			Animation.Play("Idle");
		}

		public override void Update()
		{
			Vector2 movement = Vector2.Zero;
			bool gpConnected = Input.GamepadState.IsConnected;

			if (Input.KeyboardState.IsKeyDown(Keys.D1) || gpConnected && Input.GamepadState.IsButtonDown(Buttons.LeftTrigger))
			{
				movement.X -= 1f;
				go.Animation.Play("Stealth");
				go.Renderer.IsFlipped = false;
			}
			if (Input.KeyboardState.IsKeyDown(Keys.D3) || gpConnected && Input.GamepadState.IsButtonDown(Buttons.RightTrigger))
			{
				movement.X += 1f;
				go.Animation.Play("Stealth");
				go.Renderer.IsFlipped = true;
			}

			if (Input.KeyboardState.IsKeyDown(Keys.A) || gpConnected && Input.GamepadState.ThumbSticks.Left.X < 0)
			{
				movement.X -= 1f;
				go.Animation.Play("Run");
				go.Renderer.IsFlipped = false;

			}

			if (Input.KeyboardState.IsKeyDown(Keys.D) || gpConnected && Input.GamepadState.ThumbSticks.Left.X > 0)
			{
				movement.X += 1f;
				go.Animation.Play("Run");
				go.Renderer.IsFlipped = true;

			}
			if (Input.KeyboardState.IsKeyDown(Keys.W) || gpConnected && Input.GamepadState.ThumbSticks.Left.Y > 0)
			{
				movement.Y -= 1f;
				go.Animation.Play("Climb");
			}

			if (Input.KeyboardState.IsKeyDown(Keys.S) || gpConnected && Input.GamepadState.ThumbSticks.Left.Y < 0)
			{
				movement.Y += 1f;
				go.Animation.Play("Climb");
			}

			if (movement.Length() > 0)
			{
				movement.Normalize();
				movement /= RigidBody.MInPx;
				if (go.RigidBody.Body.LinearVelocity.LengthSquared() < maxSpeed)
					go.RigidBody.ApplyImpulse(movement * moveSpeed);
			}

			else
			{
				go.Animation.Play("Idle");
			}
			if (Input.KeyboardState.IsKeyDown(Keys.F) || gpConnected && Input.GamepadState.IsButtonDown(Buttons.X))
			{
				go.Animation.Play("Stab");
			}


			if (Input.KeyboardState.IsKeyDown(Keys.Z) || Input.GamepadState.IsButtonDown(Buttons.Y))
			{
				go.Animation.Play("Hide");
			}
		}

		private static Texture2D characterSprite;

		public static GameObject CreatePlayerGO(ContentManager content, GraphicsDevice gd)
		{
			if (characterSprite == null)
			{
				characterSprite = content.Load<Texture2D>("Characters/MainCharacter");
			}

			GameObject playerGO = new GameObject();
			playerGO.AddTransform();
			playerGO.AddAnimation(characterSprite, new Vector2(30f, 30f));
			playerGO.Animation.AddAnimation("Idle", 0, 1);
			playerGO.Animation.AddAnimation("Run", 0, 4);
			playerGO.Animation.AddAnimation("Hide", 1, 1);
			playerGO.Animation.AddAnimation("Stab", 2, 1);
			playerGO.Animation.AddAnimation("Stealth", 1, 5);
			playerGO.Animation.AddAnimation("Climb", 3, 4);
			playerGO.AddRenderer(gd, SpriteTransparency.Transparent);
			playerGO.AddDynamicRigidBody(new Vector2(30f, 30f));
			playerGO.RigidBody.CollisionCategory = CollisionCats.PlayerCategory;
			playerGO.AddScript(new PlayerScript(playerGO));

			return playerGO;
		}
	}
}

