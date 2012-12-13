using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
		private bool isInVents = false;

		private PlayerState state;

		public PlayerScript(GameObject parent)
			: base(parent)
		{
			Animation.Play("Idle");
		}

		public bool HasShank
		{
			set
			{
				hasShank = value;
				Audio.Play("Pickup");
			}
		}

		public bool IsInVents
		{
			set
			{
				isInVents = value;
				RigidBody.Body.IgnoreGravity = value;

				if (value)
				{
					RigidBody.Body.LinearDamping = 10f;
					RigidBody.Body.Friction = 0f;
				}
				else
				{
					RigidBody.Body.LinearDamping = 0f;
					RigidBody.Body.Friction = 2f;
				}
			}
		}

		public bool IsAttacking
		{
			get { return state == PlayerState.Attacking; }
		}

		public override void Update()
		{
			Vector2 movement = Vector2.Zero;
			bool gpConnected = Input.GamepadState.IsConnected;

			if (!isInVents)
			{
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

				if (hasShank && Input.KeyboardState.IsKeyDown(Keys.F) || gpConnected && Input.GamepadState.IsButtonDown(Buttons.X))
				{
					Animation.Play("Stab");
					state = PlayerState.Attacking;
					Audio.Play("Shank");
				}
				if (state == PlayerState.Idle)
				{
					Animation.Play("Idle");
				}
			}
			// In vents movement
			else
			{
				if (Input.KeyboardState.IsKeyDown(Keys.D) || gpConnected && Input.GamepadState.ThumbSticks.Left.X > 0)
				{
					movement.X += 1f;
					Animation.Play("VentCrawl");
				}
				if (Input.KeyboardState.IsKeyDown(Keys.A) || gpConnected && Input.GamepadState.ThumbSticks.Left.X < 0)
				{
					movement.X -= 1f;
					Animation.Play("VentCrawl");
				}
				if (Input.KeyboardState.IsKeyDown(Keys.W) || gpConnected && Input.GamepadState.ThumbSticks.Left.Y > 0)
				{
					movement.Y += 1f;
					Animation.Play("VentCrawl");
				}
				if (Input.KeyboardState.IsKeyDown(Keys.S) || gpConnected && Input.GamepadState.ThumbSticks.Left.Y < 0)
				{
					movement.Y -= 1f;
					Animation.Play("VentCrawl");
				}

				if (movement.Length() > 0)
				{
					movement.Normalize();
					movement /= RigidBody.MInPx;
					if (RigidBody.Body.LinearVelocity.LengthSquared() < maxSpeed)
						RigidBody.ApplyImpulse(movement * moveSpeed);
					return;
				}

				Animation.Play("VentIdle");
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
			playerGO.AddAudio();
			playerGO.Audio.AddSFX("Pickup", content.Load<SoundEffect>("Sounds/PlayerSounds/Pickup"));
			playerGO.Audio.AddSFX("Shank", content.Load<SoundEffect>("Sounds/PlayerSounds/Shank"));
			playerGO.AddAnimation(characterSprite, new Vector2(28f, 30f));
			playerGO.Animation.AddAnimation("Idle", 0, 1);
			playerGO.Animation.AddAnimation("Run", 0, 4);
			playerGO.Animation.AddAnimation("Hide", 2, 1);
			playerGO.Animation.AddAnimation("Stab", 3, 1);
			playerGO.Animation.AddAnimation("Stealth", 1, 4);
			playerGO.Animation.AddAnimation("Elevator", 5, 1);
			playerGO.Animation.AddAnimation("VentIdle", 4, 1);
			playerGO.Animation.AddAnimation("VentCrawl", 4, 4);
			playerGO.AddRenderer(gd, SpriteTransparency.Transparent);
			playerGO.AddDynamicRigidBody(new Vector2(28f, 30f));
			playerGO.RigidBody.CollisionCategory = CollisionCats.PlayerCategory;
			playerGO.AddScript(new PlayerScript(playerGO));

			return playerGO;
		}
	}
}

