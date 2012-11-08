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
            par.CAnimation.Play("idle");
            
        }

        public override void Update()
        {
            if (Input.KeyboardState.IsKeyDown(Keys.D))
            {
                par.CTransform.Translate(new Vector2(2f, 0f));
                par.CAnimation.Play("run");
            }
            if (Input.KeyboardState.IsKeyDown(Keys.A))
            {
                par.CTransform.Translate(new Vector2(-2f, 0f));
            }
            if (Input.KeyboardState.IsKeyDown(Keys.S))
            {
                par.CTransform.Translate(new Vector2(0f, 2f));
            }
            if (Input.KeyboardState.IsKeyDown(Keys.W))
            {
                par.CTransform.Translate(new Vector2(0f, -2f));
            }
            // the player's special move
            //if (Input.KeyboardState.IsKeyDown(Keys.Space))
            //{

            //}

            if (Input.GamepadState.IsConnected)
            {
                if(Input.GamepadState.IsButtonDown(Buttons.LeftThumbstickRight))
                {
                    par.CTransform.Translate(new Vector2(2f, 0f));
                }
                   if(Input.GamepadState.IsButtonDown(Buttons.LeftThumbstickLeft))
                {
                    par.CTransform.Translate(new Vector2(-2f, 0f));
                }
                   if(Input.GamepadState.IsButtonDown(Buttons.LeftThumbstickUp))
                {
                    par.CTransform.Translate(new Vector2(0f, -2f));
                }
                   if(Input.GamepadState.IsButtonDown(Buttons.LeftThumbstickDown))
                {
                   par.CTransform.Translate(new Vector2(0f, 2f));
                     
                }

                //the PLayer's special move
                   //if (Input.GamepadState.IsButtonDown(Buttons.A))
                   //{

                   //}
            }
        } 
    }
}
