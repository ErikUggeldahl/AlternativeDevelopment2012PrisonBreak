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
		 public DialogBox(GameObject parent)
			 : base(parent)
        {
            par = parent;

        }
		 public DialogBox(GameObject parent, SpriteFont spriteFont)
			 : base(parent)
		 {
			 par = parent;
			 SpriteFont1 = spriteFont;
		 }
		 public SpriteFont SpriteSheet
		 {
			 get { return SpriteFont1; }
		 }
 
		public string InputText()
		{

			List<string> dogs = new List<string>();
			dogs.Add("Hi"); // Add string 1
			dogs.Add("Spitz"); // 2
			dogs.Add("Mastiff"); // 3
			dogs.Add("Finnish Spitz"); // 4
			dogs.Add("Briard"); // 5

			text = string.Join(",", dogs.ToArray());

			for (int i = 0; i < text.Length; i++)
			{
				if ((float)i % 5f == 0)
				{
					text = text.Insert(i, "\n");
				}
			}
			Console.WriteLine(text);
			return text;
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
					//FontPos.Y += 20;
					ToLong = false;
				}

				timeCounter = 0;
				subIndex++;

			}

		}
	
	}
}
