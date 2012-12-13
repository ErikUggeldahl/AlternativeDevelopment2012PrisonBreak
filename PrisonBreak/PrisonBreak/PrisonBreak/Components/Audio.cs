using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace PrisonBreak.Components
{
	public class Audio : BaseComponent
	{

        private Dictionary<string, SoundEffect> sounds;
        private SoundEffect currentSoundEffect;

		public Audio(GameObject parent)
			: base(parent)
		{
            sounds = new Dictionary<string, SoundEffect>();
		}

        public void AddAudio(string name, SoundEffect sound)
        {
            sounds.Add(name, sound);
        }

		public override void Update()
		{ 
		}

		public void Play(string toPlay)
		{
            currentSoundEffect = sounds[toPlay];
            currentSoundEffect.Play();
		}

		public void Pause()
		{
		}

		public void Stop()
		{
            currentSoundEffect.Play(0.0f, 0.0f, 0.0f);
		}
	}
}
