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
		private static Texture2D laserSprite;
		private TimeSpan cycleTime = TimeSpan.FromSeconds(3.0f);
		private TimeSpan currentTime = TimeSpan.Zero;
		private bool active = false;

		public LaserScript(GameObject parent)
			: base(parent)
		{
		}

		private bool TriggerEnter(FarseerPhysics.Dynamics.Fixture fixtureA, FarseerPhysics.Dynamics.Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
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

		public static GameObject CreateLaserGO(ContentManager content, GraphicsDevice gd)
		{
			if (laserSprite == null)
			{
				laserSprite = content.Load<Texture2D>("Obstacles/Laser");
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
