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
       Transform CTransform;
       Animation CAnimation;
       Audio CAudio;
       Collider CCollider;
       Camera CCamera;
       Renderer CRenderer;

       public GameObject()
       {
           components = new List<IComponent>();
       }

       public void AddComponent(IComponent component)
       {
           if (component.GetType() == typeof(Transform))
           {
               CTransform = (Transform)component;
           }
           else if (component.GetType() == typeof(Animation))
           {
               CAnimation = (Animation)component;
           }
           else if (component.GetType() == typeof(Audio))
           {
               CAudio = (Audio)component;
           }
           else if (component.GetType() == typeof(Collider))
           {
               CCollider = (Collider)component;
           }
           else if (component.GetType() == typeof(Camera))
           {
               CCamera = (Camera)component;
           }
           else if (component.GetType() == typeof(Renderer))
           {
               CRenderer = (Renderer)component;
           }

           components.Add(component);
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
