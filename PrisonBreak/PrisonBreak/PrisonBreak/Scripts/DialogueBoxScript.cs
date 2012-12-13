using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using PrisonBreak.Components;

namespace PrisonBreak.Scripts
{
	public class DialogueBoxScript : Script
	{
		private const int boxCharacterWidth = 29;
		private const float letterSpeed = 0.025f;

		private string character;
		private string text;
		private string workingText;
		private int subIndex;
		private float timeCounter;
		private int lineCount;

		public DialogueBoxScript(GameObject parent, string dialogueText, string character)
			: base(parent)
		{
			text = dialogueText;
			this.character = character;

			// Format with new lines
			List<string> parts = new List<string>();
			while (text.Length > boxCharacterWidth)
			{
				string toNewLine = text.Substring(0, boxCharacterWidth);
				int index = toNewLine.LastIndexOf(' ');
				parts.Add(text.Substring(0, index));
				text = text.Substring(index + 1);
			}
			parts.Add(text);
			text = string.Join("\n", parts.ToArray());

			Reset();
		}

		private void Reset()
		{
			subIndex = 0;
			timeCounter = 0;
			lineCount = 1;
			workingText = text;
		}

		public string CurrentString
		{
			get { return workingText.Substring(0, subIndex); }
		}

		public string Character
		{
			get { return character; }
		}

		public override void Update()
		{
			timeCounter += (float)GameTimeGlobal.GameTime.ElapsedGameTime.TotalSeconds;

			if (timeCounter >= letterSpeed && subIndex < workingText.Length - 1)
			{
				timeCounter = 0f;
				subIndex++;

				if (workingText[subIndex] == '\n')
				{
					if (lineCount == 4)
					{
						int firstNewLine = workingText.IndexOf('\n', 0) + 1;
						workingText = workingText.Remove(0, firstNewLine);
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

		private bool OnEnter(FarseerPhysics.Dynamics.Fixture fixtureA, FarseerPhysics.Dynamics.Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
		{
			Reset();
			DialogueRenderer.Instance.CurrentDialogue = this;
			return true;
		}

		private void OnExit(FarseerPhysics.Dynamics.Fixture fixtureA, FarseerPhysics.Dynamics.Fixture fixtureB)
		{
			DialogueRenderer.Instance.CurrentDialogue = null;
		}

		public static GameObject CreateDialogueAreaGO(string dialogue, string character, Vector2 size)
		{
			GameObject dialogueGO = new GameObject();
			dialogueGO.AddTransform();
			dialogueGO.AddTrigger(size);
			DialogueBoxScript script = new DialogueBoxScript(dialogueGO, dialogue, character);
			dialogueGO.AddScript(script);
			dialogueGO.Trigger.CollidesWith = CollisionCats.PlayerCategory;
			dialogueGO.Trigger.OnEnter += new FarseerPhysics.Dynamics.OnCollisionEventHandler(script.OnEnter);
			dialogueGO.Trigger.OnExit += new FarseerPhysics.Dynamics.OnSeparationEventHandler(script.OnExit);

			return dialogueGO;
		}
	}
}
