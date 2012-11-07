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

        public Rectangle source;

        private float time;

        static float framerate;
    //private dictionary that also hold the tuple
        private Dictionary<string, Tuple<int, int>> animationFrames;

        void Test()
        {
            // the tuple holds two items, the first is the start frame in the animation and the second one is the end frame in the animation
            Tuple<int, int> frames = new Tuple<int, int>(6, 13);

            animationFrames.Add("Walk", frames);
        }

        //private dictionary
    
        int frameIndex;

        public float FrameTime
        {
            get { return framerate; }
        }

 
        public int FrameIndex
        {
            get { return frameIndex; }
        }
        public Rectangle CurrentFrame
        {
            get { return source;}   
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
