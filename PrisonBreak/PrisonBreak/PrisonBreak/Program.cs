using System;

namespace PrisonBreak
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Game1 game = new Game1())
            {
                game.Run();
                //coles awesome
                // check me out
                //Gears of WAR!!!
            }
        }
    }
#endif
}

