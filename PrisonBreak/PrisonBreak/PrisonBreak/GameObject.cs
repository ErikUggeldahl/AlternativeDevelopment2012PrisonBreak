using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PrisonBreak
{
   public class GameObject
    {
       List<IComponent> components;
       public Transform CTransform;
       public Animation CAnimation;
       public Audio CAudio;
       public Collider CCollider;
       public Camera CCamera;
       public Renderer CRenderer;
	   public DialogBox CDialogBox;

       public GameObject()
       {
           components = new List<IComponent>();
       }

       //public void AddComponent(IComponent component)
       //{
       //    if (component.GetType() == typeof(Transform))
       //    {
       //        CTransform = (Transform)component;
       //    }
       //    else if (component.GetType() == typeof(Animation))
       //    {
       //        CAnimation = (Animation)component;
       //    }
       //    else if (component.GetType() == typeof(Audio))
       //    {
       //        CAudio = (Audio)component;
       //    }
       //    else if (component.GetType() == typeof(Collider))
       //    {
       //        CCollider = (Collider)component;
       //    }
       //    else if (component.GetType() == typeof(Camera))
       //    {
       //        CCamera = (Camera)component;
       //    }
       //    else if (component.GetType() == typeof(Renderer))
       //    {
       //        CRenderer = (Renderer)component;
       //    }

       //    components.Add(component);
       //}

       public void AddTransform()
       {
           if (CTransform == null)
           {
               CTransform = new Transform(this);
               components.Add(CTransform);
           }
       }

       public void AddAnimation(Texture2D spriteSheet, Rectangle cellSize)
       {
           if (CAnimation == null)
           {
               CAnimation = new Animation(this, spriteSheet, cellSize);
               components.Add(CAnimation);
           }
       }
	
	   
	 // still working on this

	   public void AddDialogBox(SpriteFont spriteFont)
	   {
		   if (CDialogBox == null)
		   {
			   CDialogBox = new DialogBox();
			   components.Add(CDialogBox);
		   }

	   }
       public void AddCamera(Viewport vp, bool isMain)
       {
           if (CCamera == null)
           {
              CCamera = new Camera(this, vp, isMain);
               components.Add(CCamera);
           }
       }

       public void AddRenderer(SpriteBatch sb)
       {
           if (CRenderer == null)
           {
               CRenderer = new Renderer(this, sb);
               components.Add(CRenderer);
           }
       }

       public void AddCollier()
       {
           if (CCollider == null)
           {
               CCollider = new Collider(this);
               components.Add(CCollider);
           }
       }

       public void AddAudio()
       {
           if (CAudio == null)
           {
               CAudio = new Audio(this);
               components.Add(CAudio);
           }
       }

       public void AddScript(Script script)
       {
           components.Add(script);
       }

       public void Update()
       {
           for (int i = 0; i < components.Count; i++)
           {
               components[i].Update();
           }
       }

       public void Render()
       {
           if (CRenderer != null)
           {
               CRenderer.Draw();
           }
       }

       

    }
}
