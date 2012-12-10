using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PrisonBreak.QuadTree;

namespace PrisonBreak.Components
{
	public class Camera : BaseComponent, IBounded
	{
		public static Camera MainCamera;

		private Vector2 origin;
		private Viewport viewport;

		// TODO: Adjust to real world bounds! -- Doesn't seem to have an effect??
		// Quads might not respond to transforms
		private static PrisonBreak.QuadTree.QuadTree<IBounded> quadTree = new PrisonBreak.QuadTree.QuadTree<IBounded>(new Rectangle(0, 0, 1, 1));

		public Viewport Viewport
		{
			get { return viewport; }
		}

		public Vector2 Origin
		{
			get { return origin; }
		}

		public Rectangle AABounds
		{
			// TODO: Add XY plane projection
			get
			{
				Rectangle aaBounds = viewport.Bounds;
				aaBounds.X = (int)Transform.WorldPosition.X - (int)(aaBounds.Width / 2f);
				aaBounds.Y = (int)Transform.WorldPosition.Y - (int)(aaBounds.Height / 2f);
				return aaBounds;
			}
		}

		public Camera(GameObject parent, Viewport viewport, bool isMain = false)
			: base(parent)
		{
			this.viewport = viewport;

			origin = new Vector2(viewport.Width / 2, viewport.Height / 2);

			if (isMain)
			{
				Camera.MainCamera = this;
			}
		}

		public Matrix ViewMatrix
		{
			get { return Matrix.CreateLookAt(Transform.WorldPosition, new Vector3(Transform.WorldPosition.X, Transform.WorldPosition.Y, Transform.WorldPosition.Z - 2000f), Vector3.Up); }
		}

		public static void AddRenderer(Renderer toAdd)
		{
			quadTree.Insert(toAdd);
		}

		public void Cull()
		{
			List<IBounded> toEnable = quadTree.Query(this);
			for (int i = 0; i < toEnable.Count; i++)
			{
				((Renderer)toEnable[i]).IsEnabled = true;
			}
		}

		public override void Update()
		{
		}
	}
}