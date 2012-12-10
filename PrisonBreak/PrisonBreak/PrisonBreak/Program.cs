using System;

using PrisonBreak.QuadTree.Debug;

namespace PrisonBreak
{
#if WINDOWS || XBOX
    static class Program
    {
        static bool qtDebug = false;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            if (qtDebug)
            {
                using (TestQTGame game = new TestQTGame())
                {
                    game.Run();
                }
            }
            else
            {
                using (Game1 game = new Game1())
                {
                    game.Run();
                }
            }
        }
    }
#endif
}

