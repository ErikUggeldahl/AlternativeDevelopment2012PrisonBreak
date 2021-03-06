﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrisonBreak.Components
{
    public abstract class BaseComponent : IComponent
    {
        public Animation Animation;
        public Audio Audio;
        public Camera Camera;
        public Renderer Renderer;
        public RigidBody RigidBody;
		public StaticSprite StaticSprite;
        public Transform Transform;
		public Trigger Trigger;

        private GameObject go;

		public GameObject GO
		{
			get { return go; }
		}

        public BaseComponent(GameObject parent)
        {
            go = parent;

            Animation = parent.Animation;
            Audio = parent.Audio;
            Camera = parent.Camera;
            Renderer = parent.Renderer;
            RigidBody = parent.RigidBody;
			StaticSprite = parent.StaticSprite;
            Transform = parent.Transform;
			Trigger = parent.Trigger;

            parent.ComponentAdded += UpdateReferences;
        }

        public void UpdateReferences(IComponent component)
        {
			if (component.GetType() == typeof(Animation))
				Animation = (Animation)component;
			else if (component.GetType() == typeof(Audio))
				Audio = (Audio)component;
			else if (component.GetType() == typeof(Camera))
				Camera = (Camera)component;
			else if (component.GetType() == typeof(Renderer))
				Renderer = (Renderer)component;
			else if (component.GetType() == typeof(RigidBody))
				RigidBody = (RigidBody)component;
			else if (component.GetType() == typeof(StaticSprite))
				StaticSprite = (StaticSprite)component;
			else if (component.GetType() == typeof(Transform))
				Transform = (Transform)component;
			else if (component.GetType() == typeof(Trigger))
				Trigger = (Trigger)component;
        }

        public abstract void Update();
    }
}
