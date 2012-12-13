using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using PrisonBreak.Components;

namespace PrisonBreak.Scripts
{
	public class SplashScript : Script
	{
		private static Texture2D splashTexture;
		private float sitTime = 3f;
		private float currentTime = 0f;

		public SplashScript(GameObject parent)
			: base(parent)
		{
		}

		public override void Update()
		{
			currentTime += GameTimeGlobal.DeltaSec;

			if (currentTime > sitTime)
			{
				Transform.Translate(new Vector2(0f, 100f) * GameTimeGlobal.DeltaSec);
			}
		}

		public static GameObject CreateSplashGO(ContentManager content, GraphicsDevice gd)
		{
			if (splashTexture == null)
			{
				splashTexture = content.Load<Texture2D>("SplashArt");
			}

			GameObject splashGo = new GameObject();
			splashGo.AddTransform();
			splashGo.AddStaticSprite(splashTexture);
			splashGo.AddRenderer(gd, SpriteTransparency.Opaque);

			return splashGo;
		}
	}
}