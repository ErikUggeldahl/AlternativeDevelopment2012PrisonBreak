using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Microsoft.Xna.Framework;


namespace PrisonBreak
{
    class Transform : BaseComponent
    {
        List<Transform> children;

        Transform parent;
        public Vector2 position;
        float rotation;
        float scale;

        public int Parent
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {

            }
        }

        public void Translate(Vector2 delta)
        {
            position += delta;
        }

        public void Rotate(float delta)
        {
            rotation += delta;
        }

        public void Scale(float delta)
        {
            scale += delta;
        }
        public override void Update()
        {
            
        }

    }
}
