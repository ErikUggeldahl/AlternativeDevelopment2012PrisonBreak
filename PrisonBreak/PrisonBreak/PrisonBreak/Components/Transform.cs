using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace PrisonBreak.Components
{
    public class Transform : BaseComponent
    {
        List<Transform> children;

        Transform parent;
        private Vector2 position;
        private float rotation;
        private float scale;

        //Properties
        public Transform Parent
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public Transform(GameObject parent)
            : base(parent)
        {

        }
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        public float Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        public void Translate(Vector2 delta)
        {
            position += delta;
        }

        public void Rotate(float delta)
        {
            rotation += delta;
        }

        public void LocalScale(float delta)
        {
            scale += delta;
        }

        public override void Update()
        {
        }
    }
}
