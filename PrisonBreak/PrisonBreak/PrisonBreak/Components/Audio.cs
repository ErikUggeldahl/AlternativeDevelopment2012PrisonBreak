using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Audio;

namespace PrisonBreak.Components
{
	public class Audio : BaseComponent
	{
		private Dictionary<string, SoundEffect> sounds;
		private SoundEffectInstance currentSoundEffect;

		public Audio(GameObject parent)
			: base(parent)
		{
			sounds = new Dictionary<string, SoundEffect>();
		}

		public override void Update()
		{
		}

		public void AddSFX(string name, SoundEffect sound)
		{
			sounds.Add(name, sound);
		}

		public void Play(string toPlay)
		{
			if (currentSoundEffect != null)
				currentSoundEffect.Stop();

			currentSoundEffect = sounds[toPlay].CreateInstance();
			currentSoundEffect.Play();
		}

		public void Stop()
		{
			currentSoundEffect.Stop();
		}
	}
}
