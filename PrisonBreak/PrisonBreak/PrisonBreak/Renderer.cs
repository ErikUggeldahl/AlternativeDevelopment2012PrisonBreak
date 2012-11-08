using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace PrisonBreak
{
    public class Renderer : BaseComponent
    {
        SpriteBatch sb;
        SpriteEffects flip = SpriteEffects.None;

        public Renderer(GameObject parent, SpriteBatch sb) 
            : base(parent)
        {
            this.sb = sb;
        }
        public void Draw()
        {


            sb.Draw(par.CAnimation.SpriteSheet, par.CTransform.Position, par.CAnimation.CurrentFrame, Color.White, par.CTransform.Rotation, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f); 
        }

        public override void Update()
        {

        }

    }
}
