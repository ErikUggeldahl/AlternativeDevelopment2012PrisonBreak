using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using TexturedQuad;

namespace PrisonBreak.Components
{
	public class Renderer : BaseComponent
	{
		private bool animated;

		private bool isFlipped;
		private SpriteEffects flipEffect;

		private GraphicsDevice graphicsDevice;
		private SpriteBatch sb;
		private RenderTarget2D rt;
		private Quad quad;
		private Matrix world, view, proj;
		private Texture2D currentFrame;

		private static BasicEffect shader;

		public bool IsFlipped
		{
			get { return isFlipped; }
			set
			{
				isFlipped = value;
				flipEffect = isFlipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			}
		}

		public Renderer(GameObject parent, GraphicsDevice graphicsDevice)
			: base(parent)
		{
			animated = Animation != null ? true : false;

			this.graphicsDevice = graphicsDevice;
			isFlipped = false;
			flipEffect = SpriteEffects.None;

			if (animated)
				AnimatedInit();
			else
				StaticInit();

			if (shader == null)
			{
				shader = new BasicEffect(graphicsDevice);
				shader.EnableDefaultLighting();
				shader.TextureEnabled = true;
				shader.LightingEnabled = true;
			}
		}

		private void AnimatedInit()
		{
			sb = new SpriteBatch(graphicsDevice);
			rt = new RenderTarget2D(graphicsDevice, 33, 33, false, graphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24Stencil8);
			quad = new Quad(Vector3.Zero, Vector3.Backward, Vector3.Up, Animation.CurrentFrame.Width, Animation.CurrentFrame.Height);
		}

		private void StaticInit()
		{
			quad = new Quad(Vector3.Zero, Vector3.Backward, Vector3.Up, StaticSprite.Sprite.Width, StaticSprite.Sprite.Height);
		}

		public void DrawTargets()
		{
			if (!animated)
				return;

			currentFrame = new Texture2D(graphicsDevice, 33, 33);
			graphicsDevice.SetRenderTarget(rt);

			sb.Begin(SpriteSortMode.Immediate, BlendState.Opaque, null, DepthStencilState.Default, null);
			sb.Draw(Animation.SpriteSheet, Vector2.Zero, Animation.CurrentFrame, Color.White, 0f, Vector2.Zero, 1f, flipEffect, 0f);
			sb.End();

			graphicsDevice.SetRenderTarget(null);
			currentFrame = (Texture2D)rt;
		}

		public void Draw()
		{
			world = Matrix.CreateTranslation(new Vector3(Transform.Position, Transform.Z));
			view = Matrix.CreateLookAt(new Vector3(Camera.MainCamera.Transform.Position, Camera.MainCamera.Transform.Z), new Vector3(Camera.MainCamera.Transform.Position, Camera.MainCamera.Transform.Z - 2000f), Vector3.Up);
			proj = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(60f), 16f / 9f, 0.5f, 5000f);

			shader.World = world;
			shader.View = view;
			shader.Projection = proj;
			shader.TextureEnabled = true;
			shader.Texture = animated ? currentFrame : StaticSprite.Sprite;

			EffectParameter tWorld = Game1.tEffect.Parameters["World"];
			tWorld.SetValue(world);
			EffectParameter tView = Game1.tEffect.Parameters["View"];
			tView.SetValue(view);
			EffectParameter tProj = Game1.tEffect.Parameters["Projection"];
			tProj.SetValue(proj);
			EffectParameter tTex = Game1.tEffect.Parameters["Texture"];
			if (animated)
				tTex.SetValue(currentFrame);
			else
				tTex.SetValue(StaticSprite.Sprite);

			graphicsDevice.BlendState = BlendState.AlphaBlend;
			foreach (EffectPass pass in shader.CurrentTechnique.Passes)
			{
				pass.Apply();

				graphicsDevice.DrawUserIndexedPrimitives<VertexPositionNormalTexture>
					(PrimitiveType.TriangleList, quad.Vertices, 0, 4, quad.Indexes, 0, 2);
			}

			//foreach (EffectPass pass in Game1.tEffect.CurrentTechnique.Passes)
			//{
			//    pass.Apply();

			//    graphicsDevice.DrawUserIndexedPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, quad.Vertices, 0, 4, quad.Indexes, 0, 2);
			//}
		}

		public override void Update()
		{
		}
	}
}
