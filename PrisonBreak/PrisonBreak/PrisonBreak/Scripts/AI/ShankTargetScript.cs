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
	public class ShankTargetScript : Script
	{
		private static Texture2D tysonSprite;

		private PlayerScript playerScript;

		public ShankTargetScript(GameObject parent, PlayerScript playerScript)
			: base(parent)
		{
			this.playerScript = playerScript;
		}

		private bool OnEnter(FarseerPhysics.Dynamics.Fixture fixtureA, FarseerPhysics.Dynamics.Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
		{
			if (playerScript.IsAttacking)
				Animation.Play("Die");
			return true;
		}

		private void OnStay(FarseerPhysics.Dynamics.Fixture fixtureA, FarseerPhysics.Dynamics.Fixture fixtureB)
		{
			if (playerScript.IsAttacking)
				Animation.Play("Die");
		}

		public override void Update()
		{
		}

		public static GameObject CreateTysonGO(ContentManager manager, GraphicsDevice gd, PlayerScript playerScript)
		{
			if (tysonSprite == null)
			{
				tysonSprite = manager.Load<Texture2D>("Characters/MikeTyson");
			}

			GameObject tyson = new GameObject();

			tyson.AddTransform();
			tyson.AddAnimation(tysonSprite, new Vector2(60f, 119f));
			tyson.Animation.AddAnimation("Idle", 0, 4);
			tyson.Animation.AddAnimation("Die", 1, 3, false);
			tyson.Animation.Play("Idle");
			tyson.AddRenderer(gd, SpriteTransparency.Transparent);
			tyson.AddTrigger(new Vector2(80f, 119f));
			tyson.Trigger.CollidesWith = CollisionCats.PlayerCategory;
			ShankTargetScript tysonScript = new ShankTargetScript(tyson, playerScript);
			tyson.AddScript(tysonScript);
			tyson.Trigger.OnEnter += new FarseerPhysics.Dynamics.OnCollisionEventHandler(tysonScript.OnEnter);
			tyson.Trigger.OnStay += new FarseerPhysics.Dynamics.OnSeparationEventHandler(tysonScript.OnStay);

			return tyson;
		}
	}
}
