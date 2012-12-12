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
		private const int boxCharacterWidth = 50;
		private const float letterSpeed = 0.125f;

		// To make static
		private SpriteFont font;
		private Texture2D boxSprite;

		private string output;
		private int subIndex = 0;
		private float timeCounter = 0;
		private int lineCount = 1;

		private string proxy;

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

			if (timeCounter >= letterSpeed && subIndex < output.Length - 1)
			{
				timeCounter = 0f;
				subIndex++;

				if (output[subIndex] == '\n')
				{
					if (lineCount == 4)
					{
						int firstNewLine = output.IndexOf('\n', 0) + 1;
						output = output.Remove(0, firstNewLine);
						subIndex -= firstNewLine;
						lineCount = 4;
					}
					else
					{
						lineCount += 1;
					}
				}
			}
		}
	}
}

