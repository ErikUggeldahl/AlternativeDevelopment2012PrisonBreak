using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrisonBreak
{
    public class BoxCollider:Collider
    {

        public BoxCollider(GameObject parent)
            : base(parent)
        {

        }
        private int boundingBox;
    }
}
