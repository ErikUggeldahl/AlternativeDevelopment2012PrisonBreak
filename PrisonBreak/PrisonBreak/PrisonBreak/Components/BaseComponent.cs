using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrisonBreak.Components
{
    public abstract class BaseComponent : IComponent
    {
        protected GameObject par;

        public BaseComponent(GameObject parent)
        {
            par = parent;
        }

        public abstract void Update();
    }
}
