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
		public StaticSprite StaticSprite;
		public Transform Transform;

        public event ComponentAdded ComponentAdded;

		public GameObject()
		{
			components = new List<IComponent>();
		}

		public void AddAnimation(Texture2D spriteSheet, Vector2 cellSize)
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

		public void AddRenderer(GraphicsDevice gd)
		{
			if (Renderer == null)
			{
				Renderer = new Renderer(this, gd);
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

		public void AddStaticSprite(Texture2D sprite)
		{
			if (StaticSprite == null)
			{
				StaticSprite = new StaticSprite(this, sprite);
				components.Add(StaticSprite);
			}
			ComponentAdded(StaticSprite);
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

		public void RenderTargets()
		{
			if (Renderer != null)
			{
				Renderer.DrawTargets();
			}
		}

		public void Render()
		{
			if (Renderer != null)
			{
				Renderer.Draw();
			}
		}

		public static GameObject CreateStaticPhysicsGO(GraphicsDevice graphicsDevice, Texture2D sprite)
		{
			GameObject staticGO = CreateStaticGO(graphicsDevice, sprite);
			staticGO.AddStaticRigidBody(new Vector2(sprite.Bounds.Width, sprite.Bounds.Height));

			return staticGO;
		}

		public static GameObject CreateStaticGO(GraphicsDevice graphicsDevice, Texture2D sprite)
		{
			GameObject staticGO = new GameObject();

			staticGO.AddTransform();
			staticGO.AddStaticSprite(sprite);
			staticGO.AddRenderer(graphicsDevice);

			return staticGO;
		}
	}
}
