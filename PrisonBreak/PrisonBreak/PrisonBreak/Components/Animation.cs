using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PrisonBreak.Components
{
	public class Animation : BaseComponent
	{
		#region Fields

		private Texture2D spriteSheet;
		private Rectangle source;

		private Dictionary<string, Tuple<int, int, bool>> animationFrames;

		static float framerate = 6f;
		private float frameTime;
		private int frameIndex;
		private bool played = false;

		private string currentAnimationName;
		private Tuple<int, int, bool> currentAnimation;

		#endregion

		public float FrameTime
		{
			get { return framerate; }
		}

		public Texture2D SpriteSheet
		{
			get { return spriteSheet; }
		}

		public int FrameIndex
		{
			get { return frameIndex; }
		}
		public Rectangle CurrentFrame
		{
			get { return source; }
		}

		public void AddAnimation(string name, int row, int frameCount, bool loops = true)
		{
			animationFrames.Add(name, new Tuple<int, int, bool>(row, frameCount, loops));
		}

		public Animation(GameObject parent, Texture2D spriteSheet, Vector2 cellSize)
			: base(parent)
		{
			this.spriteSheet = spriteSheet;
			source = new Rectangle(0, 0, (int)cellSize.X, (int)cellSize.Y);

			this.frameIndex = 0;
			this.frameTime = 0.0f;
			animationFrames = new Dictionary<string, Tuple<int, int, bool>>();
		}

		public void Play(string toPlay)
		{
			if (toPlay != currentAnimationName)
			{
				currentAnimation = animationFrames[toPlay];
				frameIndex = 0;
				currentAnimationName = toPlay;
				played = false;
			}
		}

		public override void Update()
		{
			if (played)
				return;

			frameTime += (float)GameTimeGlobal.GameTime.ElapsedGameTime.TotalSeconds;
			if (frameTime >= 1f / framerate)
			{
				frameTime = 0;
				frameIndex++;
			}

			// Check to see if the current frame is greater then the frame count. Ff it is, set it 0.
			if (frameIndex == currentAnimation.Item2)
			{
				if (currentAnimation.Item3)
					frameIndex = 0;
				else
				{
					frameIndex = currentAnimation.Item2 - 1;
					played = true;
				}
			}
			source.X = frameIndex * source.Width;
			source.Y = currentAnimation.Item1 * source.Height;
		}
	}
}
