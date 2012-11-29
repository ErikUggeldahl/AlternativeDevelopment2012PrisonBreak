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
	 public class DialogBox:BaseComponent
	{
		DialogBox Words;
		int timeCounter = 0;
		int subIndex = 0;
		string output;
		bool ToLong = false;
		int loopcounter = 10;
		string text;
		//public GameObject par;
		SpriteFont SpriteFont1;
        Texture2D TextBox;
         
         
         public DialogBox(GameObject parent)
			 : base(parent)
        {
            par = parent;

        }
		 public DialogBox(GameObject parent, SpriteFont spriteFont, Texture2D Box)
			 : base(parent)
		 {
			 par = parent;
			 SpriteFont1 = spriteFont;
             TextBox = Box;
             
		 }
		 public SpriteFont SpriteSheet
		 {
			 get { return SpriteFont1; }
		 }
         public Texture2D DrawBox
         {
             get { return TextBox; }
         }
         public int TextureWidth
         {
             get { return TextBox.Width; }
         }
		public string InputText()
		{

			List<string> stringList = new List<string>();
			stringList.Add("Hi"); // Add string 1
			stringList.Add("this is a test"); // 2
			stringList.Add("this is a test"); // 3
			stringList.Add("this is a test"); // 4
			stringList.Add("this is a test"); // 5

			text = string.Join(",", stringList.ToArray());

		/*	for (int i = 0; i < text.Length; i++)
			{
				if ((float)i % TextBox.Width == 0)
				{
					text = text.Insert(i, "\n");
				}
			}*/
			return text;
		}








        public string CurrentString
        {
            get
            {  
                return output.Substring(0,subIndex);
            }
            set
            {
                
            
           
                for (int i = 0; i < output.Length; i++)
                {
                    if ((float)i % TextureWidth == 0)
                    {
                        output = output.Insert(i, "\n");
                    }
                }
            
            }
        }

   
        
		public override void Update()
		{
			Words = new DialogBox(par);
			output = Words.InputText();


			timeCounter += GameTimeGlobal.GameTime.ElapsedGameTime.Milliseconds;
			if (timeCounter >= 150 && subIndex < output.Length)
			{
				if (subIndex > loopcounter)
				{
					ToLong = true;
					loopcounter += 10;
				}

				if (ToLong == true)
				{
					output += "\n";
					ToLong = false;
				}
                
				timeCounter = 0;
				subIndex++;

			}

          
           

		}
	
	}
}
