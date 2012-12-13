using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PrisonBreak.Components;

namespace PrisonBreak.Scripts
{
	public class EndScript : Script
	{
		bool playing = false;

		public EndScript(GameObject parent)
			: base(parent)
		{
			Trigger.OnEnter += new FarseerPhysics.Dynamics.OnCollisionEventHandler(OnEnter);
		}

		private bool OnEnter(FarseerPhysics.Dynamics.Fixture fixtureA, FarseerPhysics.Dynamics.Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
		{
			if (!playing)
			{
				Audio.PlayMusic();
				playing = true;
			}
			return true;
		}

		public override void Update()
		{
		}
	}
}