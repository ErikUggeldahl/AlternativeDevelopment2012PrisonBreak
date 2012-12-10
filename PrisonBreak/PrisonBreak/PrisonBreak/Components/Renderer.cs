using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PrisonBreak.QuadTree;

namespace PrisonBreak.Components
{
	public class Renderer : BaseComponent, IComparable, IBounded
	{
		private bool enabled;
		private bool animated;

		private bool isFlipped;
		private SpriteEffects flipEffect;

		private GraphicsDevice graphicsDevice;
		private SpriteBatch sb;
		private RenderTarget2D rt;
		private Quad quad;
		private Matrix world, view, proj;

		private Texture2D currentFrame;
		private Rectangle aaBounds;

		private static BasicEffect shader;

		public bool IsEnabled
		{
			get { return enabled; }
			set { enabled = value; }
		}

		public bool IsFlipped
		{
			get { return isFlipped; }
			set
			{
				isFlipped = value;
				flipEffect = isFlipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			}
		}

		// TODO: Add XY plane projection
		public Rectangle AABounds
		{
			get
			{
				aaBounds.X = (int)Transform.WorldPosition.X - (int)(aaBounds.Width / 2f);
				aaBounds.Y = (int)Transform.WorldPosition.Y - (int)(aaBounds.Height / 2f);
				return aaBounds;
			}
		}

		public Renderer(GameObject parent, GraphicsDevice graphicsDevice, bool opaque)
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

			if (opaque)
				RenderManager.Instance.AddOpaque(this);
			else
				RenderManager.Instance.AddTransparent(this);

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
			rt = new RenderTarget2D(graphicsDevice, Animation.CurrentFrame.Width, Animation.CurrentFrame.Height, false, graphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24Stencil8);
			quad = new Quad(Vector3.Zero, Vector3.Backward, Vector3.Up, Animation.CurrentFrame.Width, Animation.CurrentFrame.Height);
			RenderManager.Instance.AddAnimated(this);
			enabled = true;
		}

		private void StaticInit()
		{
			quad = new Quad(Vector3.Zero, Vector3.Backward, Vector3.Up, StaticSprite.Sprite.Width, StaticSprite.Sprite.Height);
			aaBounds = StaticSprite.Sprite.Bounds;
			Camera.AddRenderer(this);
			enabled = false;
		}

		public void DrawRenderTarget()
		{
			graphicsDevice.SetRenderTarget(rt);

			sb.Begin(SpriteSortMode.Immediate, BlendState.Opaque, null, DepthStencilState.Default, null);
			sb.Draw(Animation.SpriteSheet, Vector2.Zero, Animation.CurrentFrame, Color.White, 0f, Vector2.Zero, 1f, flipEffect, 0f);
			sb.End();

			graphicsDevice.SetRenderTarget(null);
			currentFrame = (Texture2D)rt;
		}

		public void Draw()
		{
			world = Transform.WorldMatrix;
			view = Camera.MainCamera.ViewMatrix;
			proj = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(60f), 16f / 9f, 0.5f, 5000f);

			// To remove
			Vector3 s;
			Quaternion q;
			Vector3 t;
			(world * proj).Decompose(out s, out q, out t); 

			shader.World = world;
			shader.View = view;
			shader.Projection = proj;
			shader.TextureEnabled = true;
			shader.Texture = animated ? currentFrame : StaticSprite.Sprite;

			graphicsDevice.BlendState = BlendState.AlphaBlend;
			graphicsDevice.DepthStencilState = DepthStencilState.Default;
			foreach (EffectPass pass in shader.CurrentTechnique.Passes)
			{
				pass.Apply();

				graphicsDevice.DrawUserIndexedPrimitives<VertexPositionNormalTexture>
					(PrimitiveType.TriangleList, quad.Vertices, 0, 4, quad.Indexes, 0, 2);
			}
		}

		public override void Update()
		{
			if (!animated)
				enabled = false;
		}

		public int CompareTo(Object obj)
		{
			return (int)(Transform.WorldPosition.Z - ((Renderer)obj).Transform.WorldPosition.Z);
		}
	}


	public enum SpriteTransparency
	{
		Opaque,
		Transparent
	}


	public class RenderManager
	{
		private static RenderManager instance;

		public static RenderManager Instance
		{
			get
			{
				if (instance == null)
					instance = new RenderManager();
				return instance;
			}
		}

		private List<Renderer> animated;
		private List<Renderer> opaques;
		private List<Renderer> transparents;

		private RenderManager()
		{
			animated = new List<Renderer>();
			opaques = new List<Renderer>();
			transparents = new List<Renderer>();
		}

		public void AddAnimated(Renderer r)
		{
			animated.Add(r);
		}

		public void AddOpaque(Renderer r)
		{
			opaques.Add(r);
		}

		public void AddTransparent(Renderer r)
		{
			transparents.Add(r);
		}

		public void Draw()
		{
			Camera.MainCamera.Cull();
			DrawRenderTargets();
			DrawOpaque();
			DrawTransparent();
		}

		public void DrawRenderTargets()
		{
			for (int i = 0; i < animated.Count; i++)
			{
				animated[i].DrawRenderTarget();
			}
		}

		public void DrawOpaque()
		{
			for (int i = 0; i < opaques.Count; i++)
			{
				if (opaques[i].IsEnabled)
					opaques[i].Draw();
			}
		}

		public void DrawTransparent()
		{
			transparents.Sort();
			for (int i = 0; i < transparents.Count; i++)
			{
				if (transparents[i].IsEnabled)
					transparents[i].Draw();
			}
		}
	}
}
