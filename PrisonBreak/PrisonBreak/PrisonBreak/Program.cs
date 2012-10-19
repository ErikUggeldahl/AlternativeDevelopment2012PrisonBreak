using System;

using PrisonBreak.QuadTree.QTDebug;
using PrisonBreak.Shading.ShadingDebug;

namespace PrisonBreak
{
#if WINDOWS || XBOX
    static class Program
    {
        static bool qtDebug = false;
        static bool shadingDebug = true;

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
            else if (shadingDebug)
            {
                using (TestShadingGame game = new TestShadingGame())
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

