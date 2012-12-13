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
		float moveSpeed = 40f;

		TimeSpan waitCounter = TimeSpan.Zero;
		TimeSpan waitDuration = TimeSpan.FromSeconds(2.0f);

		bool descending = true;
		float maxZDepth = -20f;

		static Texture2D fanSprite;

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
			if (descending && Transform.Z > maxZDepth)
			{
				Transform.Z -= moveSpeed * GameTimeGlobal.DeltaSec;
				Animation.Play("Idle");
			}
			else if (!descending && Transform.Z < 0f)
			{
				Transform.Z += moveSpeed * GameTimeGlobal.DeltaSec;
				Animation.Play("Idle");
			}
			else
			{
				if (descending)
				{
					descending = false;
					return;
				}
				else
				{
					Trigger.Enabled = true;
					Animation.Play("Spin");

					waitCounter += GameTimeGlobal.GameTime.ElapsedGameTime;
					if (waitCounter > waitDuration)
					{
						waitCounter = TimeSpan.Zero;
						descending = true;
						Trigger.Enabled = false;
					}
				}
			}
		}

		public static GameObject CreateFanGO(ContentManager manager, GraphicsDevice gd)
		{
			if (fanSprite == null)
			{
				fanSprite = manager.Load<Texture2D>("Obstacles/Fan");
			}

			GameObject fan = new GameObject();

			fan.AddTransform();
			fan.AddAnimation(fanSprite, new Vector2(62f, 58f));
			fan.Animation.AddAnimation("Idle", 0, 1);
			fan.Animation.AddAnimation("Spin", 0, 2);
			fan.Animation.Play("Spin");
			fan.AddRenderer(gd, SpriteTransparency.Opaque);
			fan.AddTrigger(new Vector2(62f, 58f));
			fan.Trigger.CollidesWith = CollisionCats.PlayerCategory;
			FanScript fanScript = new FanScript(fan);
			fan.AddScript(fanScript);
			fan.Trigger.OnEnter += new FarseerPhysics.Dynamics.OnCollisionEventHandler(fanScript.TriggerEnter);

			return fan;
		}
	}
}
