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

namespace PrisonBreak.Scripts
{
    public class PlayerScripts : Script
    {
        public PlayerScripts(GameObject parent) : base (parent)
        {

        }

        public override void Update()
        {
            if (Input.KeyboardState.IsKeyDown(Keys.D))
            {
                par.CTransform.Translate(new Vector2(2f, 0f));
            }
        } 
    }
}
