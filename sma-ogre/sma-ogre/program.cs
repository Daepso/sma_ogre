using System;

namespace sma_ogre
{
    class Program : OgreApp
    {
        public static void Main()
        {
            try
            {
                new Program().Launch();
            }
            catch (OperationCanceledException) 
            { 
                Environment.Exit(0);
            }
        }
    }
}

