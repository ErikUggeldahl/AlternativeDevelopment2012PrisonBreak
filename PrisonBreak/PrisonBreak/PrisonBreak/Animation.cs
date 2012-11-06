using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//tuple
//dictionary
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PrisonBreak
{
    public class Animation : BaseComponent
    {
        public Texture2D spriteSheet;
        //add property

        private float time;

        //static float framerate

        //private dictionary

       

        int frameIndex;

        public float FrameTime
        {
            get { return //framerate; }
        }

 
        public int FrameIndex
        {
            get { return frameIndex; }
        }
        public Texture2D CurrentFrame
        {
            get
            {
                return //source rect;
            }
           
        }

       //origin is under transform

        //public Animation Animation
        //{
        //    get { return Animation; }
        //}

        //public void PlayAnimation(Animation spriteSheet)
        //{
        //    if (Animation == animation)
        //        return;

        //    this.animation = animation;
        //    this.frameIndex = 0;
        //    this.time = 0.0f;
        //}

         

        public Animation(GameObject parent, Texture2D spriteSheet)
            : base(parent)
        {
            this.spriteSheet = spriteSheet;
          
            this.frameIndex = 0;
            this.time = 0.0f;
            
        }
        public override void Update()
        {

        }

    }
}
