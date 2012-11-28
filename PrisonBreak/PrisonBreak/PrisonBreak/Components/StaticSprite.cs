using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PrisonBreak.Components
{
	public class StaticSprite : BaseComponent
	{
		private Texture2D sprite;

		public Texture2D Sprite
		{
			get { return sprite; }
		}

		public StaticSprite(GameObject parent, Texture2D sprite)
			: base(parent)
		{
			this.sprite = sprite;
		}

		public override void Update()
		{
		}
	}
}
