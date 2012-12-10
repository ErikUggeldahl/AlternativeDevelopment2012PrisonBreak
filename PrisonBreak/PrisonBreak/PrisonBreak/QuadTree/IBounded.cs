using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace PrisonBreak.QuadTree
{
    public interface IBounded
    {
        Rectangle AABounds { get; }
    }
}
