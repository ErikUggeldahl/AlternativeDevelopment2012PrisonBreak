using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace PrisonBreak
{
	class GameTimeGlobal
	{
		static GameTime gameTime;
		static bool set = false;

		public static GameTime GameTime
		{
			get { return gameTime; }
			set
			{
				if (!set)
				{
					gameTime = value;
					set = true;
				}
			}
		}

		public static float DeltaSec
		{
			get { return (float)gameTime.ElapsedGameTime.TotalSeconds; }
		}
	}
}
