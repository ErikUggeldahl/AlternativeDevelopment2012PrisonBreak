using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrisonBreak.Components
{
	// TODO: Add events to diffuse updates to components so that par doesn't have to be referenced.
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
