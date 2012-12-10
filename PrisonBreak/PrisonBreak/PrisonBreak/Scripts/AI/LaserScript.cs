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
	public class LaserScript : Script
	{
		static Texture2D laserSprite;
		TimeSpan cycleTime = TimeSpan.FromSeconds(3.0f);
		TimeSpan currentTime = TimeSpan.Zero;
		bool active = false;

		public LaserScript(GameObject parent)
			: base(parent)
		{
		}

		bool TriggerEnter(FarseerPhysics.Dynamics.Fixture fixtureA, FarseerPhysics.Dynamics.Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
		{
			return true;
		}

		public override void Update()
		{
			currentTime += GameTimeGlobal.GameTime.ElapsedGameTime;
			if (currentTime >= cycleTime)
			{
				if (!active)
				{
					active = true;
					Animation.Play("On");
					Trigger.Enabled = true;
				}
				else
				{
					active = false;
					Animation.Play("Off");
					Trigger.Enabled = false;
				}

				currentTime -= cycleTime;
			}

		}

		public static GameObject CreateLaserGO(ContentManager manager, GraphicsDevice gd)
		{
			if (laserSprite == null)
			{
				laserSprite = manager.Load<Texture2D>("Laser");
			}

			GameObject laser = new GameObject();

			laser.AddTransform();
			laser.AddAnimation(laserSprite, new Vector2(32f, 16f));
			laser.Animation.AddAnimation("Off", 0, 1);
			laser.Animation.AddAnimation("On", 1, 1);
			laser.Animation.Play("On");
			laser.AddRenderer(gd, SpriteTransparency.Transparent);
			laser.AddTrigger(new Vector2(32f, 16f));
			laser.Trigger.CollidesWith = CollisionCats.PlayerCategory;
			LaserScript laserScript = new LaserScript(laser);
			laser.AddScript(laserScript);
			laser.Trigger.OnEnter += new FarseerPhysics.Dynamics.OnCollisionEventHandler(laserScript.TriggerEnter);

			return laser;
		}
	}
}
