using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Factories;

namespace PrisonBreak.Components
{
	public class Trigger : BaseComponent
	{
		private Body volume;

		public event OnCollisionEventHandler OnEnter;
		public event OnSeparationEventHandler OnExit;

		public Trigger(GameObject parent, Vector2 size)
			: base(parent)
		{
			volume = BodyFactory.CreateRectangle(RigidBody.World, size.X /  RigidBody.MInPx, size.Y / RigidBody.MInPx, 1f, Transform.Position / RigidBody.MInPx);
			volume.BodyType = BodyType.Kinematic;
			volume.IsSensor = true;
			volume.CollisionCategories = CollisionCats.TriggerCategory;

			volume.OnCollision += new OnCollisionEventHandler(HandleOnCollision);
			volume.OnSeparation += new OnSeparationEventHandler(HandleOnSeparation);
		}

		bool HandleOnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
		{
			if (OnEnter != null)
				OnEnter(fixtureA, fixtureB, contact);
			return true;
		}

		void HandleOnSeparation(Fixture fixtureA, Fixture fixtureB)
		{
			if (OnExit != null)
				OnExit(fixtureA, fixtureB);
		}

		public Category CollidesWith
		{
			set { volume.CollidesWith = value; }
		}

		public override void Update()
		{

			//Contact
			//if (volume.ContactList
		}
	}
}
