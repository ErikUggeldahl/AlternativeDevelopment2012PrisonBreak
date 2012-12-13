using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using PrisonBreak.Components;

namespace PrisonBreak.Scripts
{
	public class DialogueBoxScript : Script
	{
		private const int boxCharacterWidth = 29;
		private const float letterSpeed = 0.025f;

		private List<Tuple<string, string>> dialogues;

		private int currentDialogue;
		private string workingText;
		private int subIndex;
		private float timeCounter;
		private int lineCount;

		public DialogueBoxScript(GameObject parent, List<string> text, List<string> character)
			: base(parent)
		{
			if (text.Count != character.Count)
				throw new ArgumentException("Uneven number of text and characters.");

			dialogues = new List<Tuple<string, string>>();
			for (int i = 0; i < text.Count; i++)
			{
				List<string> parts = new List<string>();
				while (text[i].Length > boxCharacterWidth)
				{
					string toNewLine = text[i].Substring(0, boxCharacterWidth);
					int index = toNewLine.LastIndexOf(' ');
					parts.Add(text[i].Substring(0, index));
					text[i] = text[i].Substring(index + 1);
				}
				parts.Add(text[i]);
				text[i] = string.Join("\n", parts.ToArray());

				dialogues.Add(new Tuple<string, string>(text[i], character[i]));
			}

			Reset();
		}

		private void Reset()
		{
			currentDialogue = 0;
			subIndex = 0;
			timeCounter = 0;
			lineCount = 1;
			workingText = dialogues[0].Item1;
		}

		public string CurrentString
		{
			get { return workingText.Substring(0, subIndex); }
		}

		public string Character
		{
			get { return dialogues[currentDialogue].Item2; }
		}

		private bool keyUp = true;
		public override void Update()
		{
			timeCounter += (float)GameTimeGlobal.GameTime.ElapsedGameTime.TotalSeconds;

			if (timeCounter >= letterSpeed && subIndex < workingText.Length)
			{
				timeCounter = 0f;

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
				subIndex++;
			}

			if (keyUp && Input.KeyboardState.IsKeyDown(Keys.Space) && currentDialogue < dialogues.Count - 1)
			{
				currentDialogue++;
				subIndex = 0;
				timeCounter = 0;
				lineCount = 1;
				workingText = dialogues[currentDialogue].Item1;
				keyUp = false;
			}
			else if (!Input.KeyboardState.IsKeyDown(Keys.Space))
				keyUp = true;
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
			List<string> lDialogue = new List<string>(1);
			lDialogue.Add(dialogue);
			List<string> lCharacter = new List<string>(1);
			lDialogue.Add(character);
			return CreateDialogueAreaGO(lDialogue, lCharacter, size);
		}

		public static GameObject CreateDialogueAreaGO(List<string> dialogue, List<string> character, Vector2 size)
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
