using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using PrisonBreak.Components;

namespace PrisonBreak
{
    public class GameObjectManager
    {
        private List<GameObject> gameObjects;
        //private QuadTree<IBounded> qt;

        public GameObjectManager()
        {
            gameObjects = new List<GameObject>();
        }
      
        public void AddGameObject(GameObject go)
        {
            gameObjects.Add(go);
        }

        public void Update()
        {
            for (int i = 0; i < gameObjects.Count; i++)
            {
                gameObjects[i].Update();
            }

			RigidBody.WorldStep();
        }

        public void Render()
        {
			for (int i = 0; i < gameObjects.Count; i++)
			{
				gameObjects[i].RenderTargets();
			}

            for (int i = 0; i < gameObjects.Count; i++)
            {
                gameObjects[i].Render();
            }
        }

    }
}
