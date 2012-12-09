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
		const int boxCharacterWidth = 50;

		// To make static
		SpriteFont font;
		Texture2D boxSprite;
		string output;
		int subIndex = 0;
		float timeCounter = 0;
		int lineCount = 0;
		public int indexer = 0;


		public DialogueBox(GameObject parent, string dialogueText, SpriteFont spriteFont, Texture2D Box)
			: base(parent)
		{
			par = parent;
			output = dialogueText;
			font = spriteFont;
			boxSprite = Box;

			// Format with new lines
			List<string> parts = new List<string>();
			while (output.Length > boxCharacterWidth)
			{
				string toNewLine = output.Substring(0, boxCharacterWidth);
				int index = toNewLine.LastIndexOf(' ');
				parts.Add(output.Substring(0, index));
				output = output.Substring(index + 1);

				if (index > boxCharacterWidth)
				{
					output.Remove(0,boxCharacterWidth);
				}

				if (lineCount == 0)
				{

					//output.Remove(0, 50);
				//	indexer = output.IndexOf('\n');
					//output.Remove(indexer, 5);

				}
				lineCount++;
			

				
				
				
				/*

				 * you would define a line limit, say 4, that would fit in the box. Then you check how many lines are printed and if
				 * it's too many, you discard what was written before
					so you need to count how many lines you're at
				 */
				if (parts.Count == lineCount)
				{
					parts.Remove(parts[0]);
					
				}



			}
			parts.Add(output);

			output = string.Join("\n", parts.ToArray());

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

				if (lineCount == 4)
				{

					output.Remove(0, 50);
					indexer = output.IndexOf('\n');
					output.Remove(indexer, 5);
					
				}
				lineCount++;
			
			}


			}



		}
	}

