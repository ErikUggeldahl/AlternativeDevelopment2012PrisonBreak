using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using PrisonBreak.Components;

namespace PrisonBreak.Scripts
{
	public class TeleporterScript : Script
	{
		private PlayerScript player;
		private Vector2 destination;
		private bool toVents;

		public TeleporterScript(GameObject parent, PlayerScript player, Vector2 destination, bool toVents)
			: base(parent)
		{
			this.player = player;
			this.destination = destination;
			this.toVents = toVents;
		}

		public override void Update()
		{
		}

		private bool OnEnter(FarseerPhysics.Dynamics.Fixture fixtureA, FarseerPhysics.Dynamics.Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
		{
			player.Transform.Position = destination;
			player.IsInVents = toVents;
			Audio.Play("Enter");
			return true;
		}

		public static GameObject CreateTeleporterGO(ContentManager content, PlayerScript player, Vector2 destination, bool toVents)
		{
			
			GameObject teleporter = new GameObject();
			teleporter.AddTransform();
			teleporter.AddAudio();
			teleporter.Audio.AddSFX("Enter", content.Load<SoundEffect>("Sounds/Enter"));
			TeleporterScript tpScript = new TeleporterScript(teleporter, player, destination, toVents);
			teleporter.AddScript(tpScript);
			teleporter.AddTrigger(new Vector2(16f, 16f));
			teleporter.Trigger.CollidesWith = CollisionCats.PlayerCategory;
			teleporter.Trigger.OnEnter += new FarseerPhysics.Dynamics.OnCollisionEventHandler(tpScript.OnEnter);

			return teleporter;
		}
	}
}
