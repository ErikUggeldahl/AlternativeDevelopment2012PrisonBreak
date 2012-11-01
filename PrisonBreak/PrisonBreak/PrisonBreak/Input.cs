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
    
    
    public class Input 
    {
        

        public int KeyboardState
        {
            get
            {
                return KeyboardState;
            }
            set
            {
        
            }

        }

        public int MouseState
        {
            get
            {
                return MouseState;
            }
            set
            {

            }

        }

        public int GamepadState
        {
            get
            {
                return GamepadState;
            }
            set
            {

            }


        }
       
    }
}
