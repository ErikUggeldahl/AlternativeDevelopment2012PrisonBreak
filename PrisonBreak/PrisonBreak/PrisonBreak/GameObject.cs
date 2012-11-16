using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using FarseerPhysics.Dynamics;

using PrisonBreak.Components;

namespace PrisonBreak
{
    public delegate void ComponentAdded(IComponent component);

	public class GameObject
	{
		List<IComponent> components;
		public Animation Animation;
		public Audio Audio;
		public Camera Camera;
		public Renderer Renderer;
		public RigidBody RigidBody;
		public Transform Transform;

        public event ComponentAdded ComponentAdded;

		public GameObject()
		{
			components = new List<IComponent>();
		}

		public void AddAnimation(Texture2D spriteSheet, Rectangle cellSize)
		{
			if (Animation == null)
			{
				Animation = new Animation(this, spriteSheet, cellSize);
				components.Add(Animation);
			}
            ComponentAdded(Animation);
		}

		public void AddAudio()
		{
			if (Audio == null)
			{
				Audio = new Audio(this);
				components.Add(Audio);
			}
            ComponentAdded(Audio);
		}

		public void AddCamera(Viewport vp, bool isMain)
		{
			if (Camera == null)
			{
				Camera = new Camera(this, vp, isMain);
				components.Add(Camera);
			}
            ComponentAdded(Camera);
		}

		public void AddRenderer(SpriteBatch sb)
		{
			if (Renderer == null)
			{
				Renderer = new Renderer(this, sb);
				components.Add(Renderer);
			}
            ComponentAdded(Renderer);
		}

		public void AddStaticRigidBody(Vector2 size)
		{
			if (RigidBody == null)
			{
				RigidBody = new RigidBody(this, BodyType.Static, size);
				components.Add(RigidBody);
			}
            ComponentAdded(RigidBody);
		}

		public void AddDynamicRigidBody(Vector2 size)
		{
			if (RigidBody == null)
			{
				RigidBody = new RigidBody(this, BodyType.Dynamic, size);
				components.Add(RigidBody);
			}
            ComponentAdded(RigidBody);
		}

		public void AddTransform()
		{
			if (Transform == null)
			{
				Transform = new Transform(this);
				components.Add(Transform);
			}
            ComponentAdded(Transform);
		}

		public void AddScript(Script script)
		{
			components.Add(script);
		}

		public void Update()
		{
			for (int i = 0; i < components.Count; i++)
			{
				components[i].Update();
			}
		}

		public void Render()
		{
			if (Renderer != null)
			{
				Renderer.Draw();
			}
		}
	}
}
