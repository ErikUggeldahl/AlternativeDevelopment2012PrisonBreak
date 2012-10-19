using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace PrisonBreak.QuadTree.Debug
{
    class DebugGO : IBounded
    {
        Rectangle bounds;

        public Rectangle AABounds
        {
            get { return bounds; }
        }

        public DebugGO(Rectangle bounds)
        {
            this.bounds = bounds;
        }

        public void SetPos(Vector2 pos)
        {
            bounds.X = (int)pos.X - bounds.Width / 2;
            bounds.Y = (int)pos.Y - bounds.Height / 2;
        }

        public void AddSize(int factor)
        {
            float ratio = bounds.Width / bounds.Height;
            bounds.Width += (int)(factor * ratio);
            bounds.Height += (int)(factor / ratio);
        }
    }
}
