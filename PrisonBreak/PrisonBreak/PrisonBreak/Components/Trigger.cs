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
        private bool enabled;

		public event OnCollisionEventHandler OnEnter;
		public event OnSeparationEventHandler OnExit;

		public Trigger(GameObject parent, Vector2 size)
			: base(parent)
		{
			volume = BodyFactory.CreateRectangle(RigidBody.World, size.X /  RigidBody.MInPx, size.Y / RigidBody.MInPx, 1f, Transform.Position / RigidBody.MInPx);
			volume.BodyType = BodyType.Kinematic;
			volume.IsSensor = true;
			volume.CollisionCategories = CollisionCats.TriggerCategory;

            enabled = true;

			volume.OnCollision += new OnCollisionEventHandler(HandleOnCollision);
			volume.OnSeparation += new OnSeparationEventHandler(HandleOnSeparation);
		}

		public Body Volume
		{
			get { return volume; }
		}

        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

		private bool HandleOnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
		{
			if (enabled && OnEnter != null)
				OnEnter(fixtureA, fixtureB, contact);
			return true;
		}

		private void HandleOnSeparation(Fixture fixtureA, Fixture fixtureB)
		{
			if (enabled && OnExit != null)
				OnExit(fixtureA, fixtureB);
		}

		public Category CollidesWith
		{
			set { volume.CollidesWith = value; }
		}

		public override void Update()
		{
		}
	}
}
