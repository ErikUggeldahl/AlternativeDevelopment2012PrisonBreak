using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace PrisonBreak.Components
{
	public class Renderer : BaseComponent
	{
		SpriteBatch sb;
		private bool isFlipped;
		private SpriteEffects flipEffect = SpriteEffects.None;

		public bool IsFlipped
		{
			get { return isFlipped; }
			set
			{
				isFlipped = value;
				flipEffect = isFlipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			}
		}

		public Renderer(GameObject parent, SpriteBatch sb)
			: base(parent)
		{
			this.sb = sb;
			isFlipped = false;
		}

		public void Draw()
		{
			Vector2 positionWithOffset = Transform.Position - new Vector2(Animation.CurrentFrame.Width / 2f, Animation.CurrentFrame.Height / 2f);
			sb.Draw(Animation.SpriteSheet, positionWithOffset, Animation.CurrentFrame, Color.White, Transform.Rotation, Vector2.Zero, 1f, flipEffect, 0.0f);
			//sb.Draw(Animation.SpriteSheet, positionWithOffset, Animation.CurrentFrame, Color.White, Transform.Rotation, Vector2.Zero, (float)Math.Exp((float)Transform.Z), flipEffect, 0.0f);
		}

		public override void Update()
		{
		}
	}
}
