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

		static BasicEffect shader;

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

			quad = new Quad(Vector3.Zero, Vector3.Backward, Vector3.Up, Animation.SpriteSheet.Width, Animation.SpriteSheet.Height);

			if (shader == null)
			{
				shader = new BasicEffect(graphicsDevice);
				shader.EnableDefaultLighting();
				shader.TextureEnabled = true;
			}
		}

		public void Draw()
		{
			// Old
			//Vector2 positionWithOffset = Transform.Position - new Vector2(Animation.CurrentFrame.Width / 2f, Animation.CurrentFrame.Height / 2f);
			//sb.Draw(Animation.SpriteSheet, positionWithOffset, Animation.CurrentFrame, Color.White, Transform.Rotation, Vector2.Zero, 1f, flipEffect, 0.0f);
			//sb.Draw(Animation.SpriteSheet, positionWithOffset, Animation.CurrentFrame, Color.White, Transform.Rotation, Vector2.Zero, (float)Math.Exp((float)Transform.Z), flipEffect, 0.0f);

			// New
			view = Matrix.CreateLookAt(new Vector3(Camera.MainCamera.Transform.Position, Camera.MainCamera.Transform.Z), new Vector3(Camera.MainCamera.Transform.Position, Camera.MainCamera.Transform.Z - 2000f), Vector3.Up);// *
				//Matrix.CreateReflection(new Plane(Vector3.UnitY, 0f));
			proj = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(60f), 16f / 9f, 0.5f, 5000f);

			shader.World = Matrix.CreateTranslation(new Vector3(Transform.Position, Transform.Z));
			shader.View = view;
			shader.Projection = proj;
			shader.TextureEnabled = true;
			shader.Texture = Animation.SpriteSheet;

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
