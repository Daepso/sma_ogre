using Mogre;
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

        protected override void CreateScene()
        {
            mSceneMgr.AmbientLight = new ColourValue(0.5f, 0.5f, 0.5f);

            AgentFactory ogreFactory = AgentFactory.OgreFactory(mSceneMgr);

            Agent ogre  = ogreFactory.makeAgent(false);
            Agent ogre2 = ogreFactory.makeAgent(true );
            Agent ogre3 = ogreFactory.makeAgent(true );
        }

        protected override void UpdateScene(FrameEvent evt)
        {
        }
    }
}

