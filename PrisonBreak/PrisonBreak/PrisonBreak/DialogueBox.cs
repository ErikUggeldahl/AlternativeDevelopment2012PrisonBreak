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
	public class DialogueBox : BaseComponent
	{
		const int boxCharacterWidth = 19;

		// To make static
		SpriteFont font;
		Texture2D boxSprite;
		char newLine = '\n';
		char space = ' ';
		string output;
		int subIndex = 0;
		float timeCounter = 0;

		public DialogueBox(GameObject parent, string dialogueText, SpriteFont spriteFont, Texture2D Box)
			: base(parent)
		{
			par = parent;
			output = dialogueText;
			font = spriteFont;
			boxSprite = Box;

			// Format with new lines
			for (int i = 0; i < output.Length; i++)
			{
				if (i % boxCharacterWidth == 0)
				{
					if ( output[i] == newLine && output[i+1] == space)
					{
						output.Remove(i + 1);
	
					}
					if (output[i] == newLine && output[i + 1] != space)
					{
						i = output.LastIndexOf(space);
						output.Insert(i, "\n");

					}
					output = output.Insert(i, "\n");
				}
			}
		}

		public SpriteFont Font
		{
			get { return font; }
		}

		public Texture2D BoxSprite
		{
			get { return boxSprite; }
		}

		public string CurrentString
		{
			get
			{
				return output.Substring(0, subIndex);
			}
		}

		public override void Update()
		{
			timeCounter += (float)GameTimeGlobal.GameTime.ElapsedGameTime.TotalSeconds;
			if (timeCounter >= 0.25f && subIndex < output.Length)
			{
				timeCounter = 0f;
				subIndex++;
			}
		}
	}
}
