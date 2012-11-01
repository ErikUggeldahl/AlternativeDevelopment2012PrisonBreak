using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PrisonBreak
{
    public class Animation : BaseComponent
    {
        private Texture2D spriteSheet;


        public Texture2D CurrentFrame
        {
            get
            {
                return spriteSheet;
            }
            set
            {
                spriteSheet = value;
            }
        }

        public Animation(GameObject parent, Texture2D spriteSheet)
            : base(parent)
        {
            this.spriteSheet = spriteSheet;
        }
        public override void Update()
        {

        }

    }
}
