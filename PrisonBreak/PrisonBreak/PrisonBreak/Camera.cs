using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace PrisonBreak
{
    public class Camera : BaseComponent
    {

        public static Camera MainCamera;

        // Get the camera's viewport
        public Viewport viewPort;

        // get the camera's limits
        //private Rectangle limits;

        // the origin or center of the screen
        public Vector2 Origin { get; set; }

        /*public Rectangle Limits
        {
            get
            {
                return limits;
            }
            set
            {
                if (value != null)
                {
                    // if out value is not null we will find the limit
                    if (limits == null)
                        limits = new Rectangle();
                    limits.X = value.X;
                    limits.Y = value.Y;

                    limits.Width = Math.Max(viewPort.Width, value.Width);
                    limits.Height = Math.Max(viewPort.Height, value.Height);
                }
                else
                {
                    limits = Rectangle.Empty;
                }

            }
        }*/

        public Matrix viewMatrix
        {
            get
            {
                return Matrix.CreateTranslation(new Vector3(-par.CTransform.Position, 0)) *
                       Matrix.CreateTranslation(new Vector3(-Origin, 0)) *
                       Matrix.CreateRotationZ(par.CTransform.Rotation) *
                       Matrix.CreateScale(1f, 1f, 1f) *
                       Matrix.CreateTranslation(new Vector3(Origin, 0));
            }
        }

        public Camera(GameObject parent, Viewport vp, bool isMain) : base(parent)
        {
            // set the viewport
            viewPort = vp;

            //Calculate the origin, center of the screen
            Origin = new Vector2(viewPort.Width / 2, viewPort.Height / 2);

            if (isMain)
            {
                Camera.MainCamera = this;
            }
        }

        public void Cull()
        {

        }

        public override void Update()
        {

        }
    }
}
    

