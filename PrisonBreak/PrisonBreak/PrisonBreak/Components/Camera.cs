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
		// TODO: Add camera Z

		public static Camera MainCamera;

		private Viewport viewport;

		public Viewport Viewport
		{
			get { return viewport; }
		}

		// get the camera's limits
		//private Rectangle limits;

		// the origin or center of the screen
		public Vector2 Origin { get; set; }

		/*public Rectangle Limits
		{
			get
			{
				return limits;
			}
			set
			{
				if (value != null)
				{
					// if out value is not null we will find the limit
					if (limits == null)
						limits = new Rectangle();
					limits.X = value.X;
					limits.Y = value.Y;

					limits.Width = Math.Max(viewPort.Width, value.Width);
					limits.Height = Math.Max(viewPort.Height, value.Height);
				}
				else
				{
					limits = Rectangle.Empty;
				}

			}
		}*/

		public Matrix ViewMatrix
		{
			get
			{
				return Matrix.CreateTranslation(new Vector3(-go.Transform.Position, 0)) *
					   Matrix.CreateTranslation(new Vector3(-Origin, 0)) *
					   Matrix.CreateRotationZ(go.Transform.Rotation) *
					   Matrix.CreateScale(1f, 1f, 1f) *
					   Matrix.CreateTranslation(new Vector3(Origin, 0));
			}
		}

		public Camera(GameObject parent, Viewport vp, bool isMain = false)
			: base(parent)
		{
			// set the viewport
			viewport = vp;

			//Calculate the origin, center of the screen
			Origin = new Vector2(viewport.Width / 2, viewport.Height / 2);

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


