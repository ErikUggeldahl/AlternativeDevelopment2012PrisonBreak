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
	public enum PlayerState
	{
		Idle, Walking, Crouching, Stealthing, Climbing, Attacking
	}

	public class PlayerScript : Script
	{
		private float moveSpeed = 40f;
		private float maxSpeed = 150f;

		private bool hasShank = false;

		private PlayerState state;

		public PlayerScript(GameObject parent)
			: base(parent)
		{
			Animation.Play("Idle");
		}

		public bool HasShank
		{
			set { hasShank = value; }
		}

		public override void Update()
		{
			Vector2 movement = Vector2.Zero;
			bool gpConnected = Input.GamepadState.IsConnected;

			bool canStealth = state == PlayerState.Crouching || state == PlayerState.Stealthing;

			if (Input.KeyboardState.IsKeyDown(Keys.D) || gpConnected && Input.GamepadState.ThumbSticks.Left.X > 0)
			{
				movement.X += 1f;
				Renderer.IsFlipped = true;

				if (canStealth)
				{
					Animation.Play("Stealth");
					state = PlayerState.Stealthing;
				}
				else
				{
					Animation.Play("Run");
					state = PlayerState.Walking;
				}
			}

			if (Input.KeyboardState.IsKeyDown(Keys.A) || gpConnected && Input.GamepadState.ThumbSticks.Left.X < 0)
			{
				movement.X -= 1f;
				Renderer.IsFlipped = false;

				if (canStealth)
				{
					Animation.Play("Stealth");
					state = PlayerState.Stealthing;
				}
				else
				{
					Animation.Play("Run");
					state = PlayerState.Walking;
				}
			}

			else if (Input.KeyboardState.IsKeyDown(Keys.A) || gpConnected && Input.GamepadState.ThumbSticks.Left.X < 0)
			{
				movement.X -= 1f;
				Animation.Play("Run");
				Renderer.IsFlipped = false;
				state = PlayerState.Walking;
			}

			if (movement.Length() > 0)
			{
				movement.Normalize();
				movement /= RigidBody.MInPx;
				if (RigidBody.Body.LinearVelocity.LengthSquared() < maxSpeed)
					RigidBody.ApplyImpulse(movement * moveSpeed);
				return;
			}

			state = PlayerState.Idle;
			if (Input.KeyboardState.IsKeyDown(Keys.LeftControl) || gpConnected && Input.GamepadState.IsButtonDown(Buttons.Y))
			{
				Animation.Play("Hide");
				state = PlayerState.Crouching;
			}

			if (Input.KeyboardState.IsKeyDown(Keys.W) || gpConnected && Input.GamepadState.ThumbSticks.Left.Y > 0)
			{
				movement.Y += 1f;
				Animation.Play("Climb");
				state = PlayerState.Climbing;
			}
			if (hasShank && Input.KeyboardState.IsKeyDown(Keys.F) || gpConnected && Input.GamepadState.IsButtonDown(Buttons.X))
			{
				Animation.Play("Stab");
				state = PlayerState.Attacking;
			}
			if (state == PlayerState.Idle)
			{
				Animation.Play("Idle");
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

