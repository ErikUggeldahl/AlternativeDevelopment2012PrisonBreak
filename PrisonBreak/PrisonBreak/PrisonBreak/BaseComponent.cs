using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PrisonBreak
{
    public abstract class BaseComponent : IComponent
    {

        public abstract void Update();
        public GameTime gameTime;
       
        public Vector2 Position;
        public float Rotation;

        public int Camera
        {
            get
            {
                return Camera;
            }
            set
            {
            }
        }

    }
}
