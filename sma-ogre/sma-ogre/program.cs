using System;

namespace sma_ogre
{
    class Program : OgreApp
    {
        public static void Main()
        {
            try
            {
                CreateRoot();
                DefineResources();
                CreateRenderSystem();
                CreateRenderWindow(); 
                InitializeResources();
                InitializeInput();
                CreateScene();
                CreateFrameListeners();
                EnterRenderLoop();
            }
            catch (OperationCanceledException) 
            { 
                Environment.Exit(0);
            }
        }
    }
}

