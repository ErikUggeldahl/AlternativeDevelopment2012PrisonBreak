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
        // Get the camera's viewport
        public Viewport viewPort;

        public Vector2 position;

        // get the camera's limits
        private Rectangle limits;

        // the origin or center of the screen
        public Vector2 Origin { get; set; }

        public Rectangle Limits
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
        }

        public Vector2 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
                //If there's a limit set there is no zoom or rotation clamp the position
                if (Limits != null)
                {
                    position.X = MathHelper.Clamp(Position.X, limits.X, limits.X + limits.Width - viewPort.Width);
                    position.Y = MathHelper.Clamp(Position.Y, limits.Y, limits.Y + limits.Height - viewPort.Height);

                }
            }
        }

        public Matrix viewMatrix
        {
            get
            {
                return Matrix.CreateTranslation(new Vector3(-Position, 0)) *
                       Matrix.CreateTranslation(new Vector3(-Origin, 0)) *
                       Matrix.CreateRotationZ(Rotation) *
                       Matrix.CreateTranslation(new Vector3(Origin, 0));
            }
        }

        public Camera(Viewport vp)
        {
            // set the viewport
            viewPort = vp;

            //Calculate the origin, center of the screen
            Origin = new Vector2(viewPort.Width / 2, viewPort.Height / 2);


        }


        public void LookAt(Vector2 lookAtPosition)
        {
            this.Position = lookAtPosition - Origin;
        }

        public void Move(Vector2 displacment, bool respectRotation = false)
        {
            if (respectRotation)
            {
                displacment = Vector2.Transform(displacment, Matrix.CreateRotationZ(-Rotation));
            }
            this.Position += displacment;
        }



        public void Cull()
        {

        }

        public override void Update()
        {

        }
    }
}
    

