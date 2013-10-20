using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Mogre;

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

