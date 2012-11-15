using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrisonBreak.Components
{
    public abstract class BaseComponent : IComponent
    {
        protected GameObject go;

		// TODO: Remove after component references have been added
		public GameObject GO
		{
			get { return go; }
		}

        public BaseComponent(GameObject parent)
        {
            go = parent;
        }

        public abstract void Update();
    }
}
