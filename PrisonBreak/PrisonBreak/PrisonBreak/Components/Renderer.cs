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
		// Old
		SpriteBatch sb;
		private bool isFlipped;
		private SpriteEffects flipEffect = SpriteEffects.None;

		// New
		GraphicsDevice graphicsDevice;
		public Quad quad;
		Matrix view, proj;
		public BasicEffect shader;

		public Texture2D testTexture;

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
			// Old
			//this.sb = sb;
			//isFlipped = false;

			// New
			this.graphicsDevice = graphicsDevice;

			quad = new Quad(Vector3.Zero, Vector3.Backward, Vector3.Up, 1f, 1f);

			view = Matrix.CreateLookAt(new Vector3(0, 0, 5), Vector3.Zero, Vector3.Up);
			proj = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(60f), 16f / 9f, 0.5f, 1000f);

			shader = new BasicEffect(graphicsDevice);
			shader.EnableDefaultLighting();
			shader.World = Matrix.CreateScale(1) * Matrix.CreateTranslation(Vector3.Zero);
			shader.View = view;
			shader.Projection = proj;
			shader.TextureEnabled = true;
			//shader.Texture = testTexture;
		}

		public void Draw()
		{
			// Old
			//Vector2 positionWithOffset = Transform.Position - new Vector2(Animation.CurrentFrame.Width / 2f, Animation.CurrentFrame.Height / 2f);
			//sb.Draw(Animation.SpriteSheet, positionWithOffset, Animation.CurrentFrame, Color.White, Transform.Rotation, Vector2.Zero, 1f, flipEffect, 0.0f);
			//sb.Draw(Animation.SpriteSheet, positionWithOffset, Animation.CurrentFrame, Color.White, Transform.Rotation, Vector2.Zero, (float)Math.Exp((float)Transform.Z), flipEffect, 0.0f);

			// New
			

			foreach (EffectPass pass in shader.CurrentTechnique.Passes)
			{
				pass.Apply();

				graphicsDevice.DrawUserIndexedPrimitives<VertexPositionNormalTexture>
					(PrimitiveType.TriangleList, quad.Vertices, 0, 4, quad.Indexes, 0, 2);
			}
		}

		public override void Update()
		{
		}
	}
}
