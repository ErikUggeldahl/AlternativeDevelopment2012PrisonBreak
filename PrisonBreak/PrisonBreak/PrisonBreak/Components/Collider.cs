using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace PrisonBreak.Components
{
	public class Collider : BaseComponent
	{
		public Rectangle BoundingBox = Rectangle.Empty;

		public Collider(GameObject parent)
			: base(parent)
		{
		}

		public override void Update()
		{
			BoundingBox.X = (int)go.Transform.Position.X;
			BoundingBox.Y = (int)go.Transform.Position.Y;
		}
	}
}
