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
        #region Feilds
        private Texture2D spriteSheet;

        private Rectangle source;

        private float frameTime;

        private int frameIndex;

        private string currentAnimationName;

        static float framerate = 6f;
        #endregion
        //private dictionary that also hold the tuple
        private Dictionary<string, Tuple<int, int>> animationFrames;

        Tuple<int, int> currentAnimation;
        void Test()
        {
            // the tuple holds two items, the first is the start frame in the animation and the second one is the end frame in the animation
            Tuple<int, int> frames = new Tuple<int, int>(6, 13);

            animationFrames.Add("Walk", frames);
        }

        //private dictionary
    
        

        public float FrameTime
        {
            get { return framerate; }
        }

        public Texture2D SpriteSheet
        {
            get { return spriteSheet; }
        }
 
        public int FrameIndex
        {
            get { return frameIndex; }
        }
        public Rectangle CurrentFrame
        {
            get { return source;}   
        }



        public void AddAnimation(string name, int row, int frameCount)
        {
            animationFrames.Add(name, new Tuple<int, int>(row, frameCount));
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

         

        public Animation(GameObject parent, Texture2D spriteSheet, Rectangle cellSize)
            : base(parent)
        {
            this.spriteSheet = spriteSheet;
            source = cellSize;

            this.frameIndex = 0;
            this.frameTime = 0.0f;
            animationFrames = new Dictionary<string, Tuple<int, int>>();
           
        }

        public void Play(string toPlay)
        {
            if (toPlay != currentAnimationName)
            {
                currentAnimation = animationFrames[toPlay];
                frameIndex = 0;
                currentAnimationName = toPlay;
            }
          

        }
        public override void Update()
        {
            frameTime += (float)GameTimeGlobal.GameTime.ElapsedGameTime.TotalSeconds;
            if (frameTime >= 1f / framerate)
            {
                frameTime = 0;
                frameIndex++;
            }

            //check to see if the current from is greater then the frame count. if it is set it 0.
          
            if (frameIndex == currentAnimation.Item2 )
            {
                frameIndex = 0;
            }

            source.X = frameIndex * source.Width;
            source.Y = currentAnimation.Item1 * source.Height;
           
        }

    }
}
