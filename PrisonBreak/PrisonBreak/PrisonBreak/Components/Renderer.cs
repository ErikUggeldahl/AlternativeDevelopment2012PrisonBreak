using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using PrisonBreak.QuadTree;
using PrisonBreak.Scripts;

namespace PrisonBreak.Components
{
	public class Renderer : BaseComponent, IComparable, IBounded
	{
		private bool enabled;
		private bool qtVisible;
		private bool animated;

		private bool isFlipped;
		private SpriteEffects flipEffect;

		private GraphicsDevice graphicsDevice;
		private SpriteBatch sb;
		private RenderTarget2D rt;
		private Quad quad;
		private Matrix world;
		private static Matrix view, proj;

		private Texture2D currentFrame;
		private Rectangle aaBounds;

		private static BasicEffect shader;

		public bool IsEnabled
		{
			get { return enabled; }
			set { enabled = value; }
		}

		public bool IsQTVisible
		{
			get { return qtVisible; }
			set { qtVisible = value; }
		}

		public bool IsFlipped
		{
			get { return isFlipped; }
			set
			{
				if (isFlipped != value)
				{
					Transform.Flip();
					isFlipped = value;
					flipEffect = isFlipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
				}
			}
		}

		// TODO: Add XY plane projection
		public Rectangle AABounds
		{
			get
			{
				aaBounds.X = (int)Transform.WorldPosition.X - (int)(aaBounds.Width / 2f);
				aaBounds.Y = (int)Transform.WorldPosition.Y - (int)(aaBounds.Height / 2f);
				//return aaBounds;
				//Vector3 point1 = (Matrix.CreateTranslation(new Vector3((float)aaBounds.X, (float)aaBounds.Y, Transform.Z)) * view * proj).Translation;
				Vector3 point1 = (Matrix.CreateTranslation(new Vector3((float)aaBounds.X, (float)aaBounds.Y, Transform.Z))).Translation;
				Vector3 point2 = (Matrix.CreateTranslation(new Vector3((float)aaBounds.X + aaBounds.Width, (float)aaBounds.Y + aaBounds.Height, Transform.Z))).Translation;

				Rectangle projBounds = new Rectangle((int)point1.X, (int)point1.Y, (int)(point2.X - point1.X), (int)(point2.Y - point1.Y));
				return projBounds;
			}
		}

		public Renderer(GameObject parent, GraphicsDevice graphicsDevice, bool opaque)
			: base(parent)
		{
			if (view == null)
				CalcViewProjMatricies();

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
			qtVisible = true;
		}

		private void StaticInit()
		{
			quad = new Quad(Vector3.Zero, Vector3.Backward, Vector3.Up, StaticSprite.Sprite.Width, StaticSprite.Sprite.Height);
			aaBounds = StaticSprite.Sprite.Bounds;
			Camera.AddRenderer(this);
			enabled = true;
			qtVisible = false;
		}

		public void CalcWorldMatrix()
		{
			world = Transform.WorldMatrix;
		}

		public static void CalcViewProjMatricies()
		{
			view = Camera.MainCamera.ViewMatrix;
			proj = Camera.MainCamera.ProjMatrix;
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

		public void ClearBG()
		{
			graphicsDevice.Clear(Color.Black);
		}

		public void Draw()
		{
			shader.World = world;
			//shader.World = Transform.WorldMatrix;
			shader.View = view;
			shader.Projection = proj;
			shader.TextureEnabled = true;
			shader.Texture = animated ? currentFrame : StaticSprite.Sprite;
			shader.Texture.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;	// Set the texture filter to point to remove magnification blur

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
				qtVisible = false;
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
			CalcMatricies();
			Camera.MainCamera.Cull();
			DrawRenderTargets();

			if (opaques.Count > 0)
				opaques[0].ClearBG();

			DrawOpaque();
			DrawTransparent();
			DialogueRenderer.Instance.DrawDialogue();
		}

		public void CalcMatricies()
		{
			Renderer.CalcViewProjMatricies();
			for (int i = 0; i < opaques.Count; i++)
			{
				opaques[i].CalcWorldMatrix();
			}
			for (int i = 0; i < transparents.Count; i++)
			{
				transparents[i].CalcWorldMatrix();
			}
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
				if (opaques[i].IsEnabled && opaques[i].IsQTVisible)
					opaques[i].Draw();
			}
		}

		public void DrawTransparent()
		{
			transparents.Sort();
			for (int i = 0; i < transparents.Count; i++)
			{
				if (transparents[i].IsEnabled && transparents[i].IsQTVisible)
					transparents[i].Draw();
			}
		}
	}


	public class DialogueRenderer
	{
		private static DialogueRenderer instance;

		public static DialogueRenderer Instance
		{
			get
			{
				if (instance == null)
					instance = new DialogueRenderer();
				return instance;
			}
		}

		private SpriteFont font;
		private Texture2D boxTexture;
		private SpriteBatch sb;

		private DialogueBoxScript currentDialogue;
		private Vector2 boxPosition = new Vector2(100f, 50f);
		private Vector2 portraitPosition = new Vector2(122f, 72f);
		private Vector2 textPosition = new Vector2(410f, 80f);
		private Dictionary<string, Texture2D> portraits;

		public DialogueBoxScript CurrentDialogue
		{
			set { currentDialogue = value; }
		}

		public DialogueRenderer()
		{
		}

		public void Initialize(ContentManager content, GraphicsDevice gd)
		{
			font = content.Load<SpriteFont>("Font/DialogueFont");
			boxTexture = content.Load<Texture2D>("Dialogue/DialogueBox");
			sb = new SpriteBatch(gd);

			portraits = new Dictionary<string, Texture2D>(8);
			portraits.Add("Bowser", content.Load<Texture2D>("Dialogue/Bowser"));
			portraits.Add("DK", content.Load<Texture2D>("Dialogue/DK"));
			portraits.Add("Ganon", content.Load<Texture2D>("Dialogue/Ganon"));
			portraits.Add("MotherBrain", content.Load<Texture2D>("Dialogue/MotherBrain"));
			portraits.Add("Player", content.Load<Texture2D>("Dialogue/Player"));
			portraits.Add("Tyson", content.Load<Texture2D>("Dialogue/Tyson"));
			portraits.Add("Wily", content.Load<Texture2D>("Dialogue/Wily"));
		}

		public void DrawDialogue()
		{
			if (currentDialogue == null)
				return;

			sb.Begin();
			sb.Draw(boxTexture, boxPosition, Color.White);
			sb.Draw(portraits[currentDialogue.Character], portraitPosition, Color.White);
			sb.DrawString(font, currentDialogue.CurrentString, textPosition, Color.White);
			sb.End();
		}
	}
}