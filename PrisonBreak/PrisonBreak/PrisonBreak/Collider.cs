using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PrisonBreak
{
    public class Collider : BaseComponent
    {
        public Rectangle BoundingBox = Rectangle.Empty;

        public void Collide()
        {

        }

        public override void Update()
        {
            BoundingBox.X = (int)Position.X;
            BoundingBox.Y = (int)Position.Y;
        }
    }
}
