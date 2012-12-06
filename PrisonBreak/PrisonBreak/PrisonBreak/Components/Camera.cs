using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PrisonBreak.Components
{
	public class Camera : BaseComponent
	{
		public static Camera MainCamera;

		private Vector2 origin;
		private Viewport viewport;

		public Viewport Viewport
		{
			get { return viewport; }
		}

		public Vector2 Origin
		{
			get { return origin; }
		}

		public Matrix ViewMatrix
		{
			get
			{
				return Matrix.CreateTranslation(new Vector3(-Origin, 0f)) *
					Matrix.CreateScale(1f) *
					Matrix.CreateTranslation(new Vector3(Transform.Offset, Transform.Z)) *
					Matrix.CreateFromQuaternion(Transform.Rotation) *
					Matrix.CreateLookAt(new Vector3(Transform.Position, Transform.Z), Vector3.Zero, Vector3.Up);
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

		public void Cull()
		{
		}

		public override void Update()
		{
		}
	}
}


