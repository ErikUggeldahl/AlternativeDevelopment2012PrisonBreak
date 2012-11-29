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
        private static MouseState mouseState;
        private static KeyboardState keyboardState;
        private static GamePadState gamePadState;

        public static KeyboardState KeyboardState
        {
            get { return keyboardState; }
            set { keyboardState = value; }
        }

        public static MouseState MouseState
        {
            get { return mouseState; }
            set { mouseState = value; }
        }

        public static GamePadState GamepadState
        {
            get { return gamePadState; }
            set { gamePadState = value; }
        }

        public static void Update()
        {
            mouseState = Mouse.GetState();
            gamePadState = GamePad.GetState(PlayerIndex.One);
            keyboardState = Keyboard.GetState();
        }
    }
}
