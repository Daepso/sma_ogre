using Mogre;
using System;

namespace sma_ogre
{
    class Program : OgreApp
    {
        AgentFactory mOgreFactory;

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

            mOgreFactory = AgentFactory.OgreFactory(mSceneMgr);

            mOgreFactory.MakeNumAgents(100, true);
        }

        protected override void UpdateScene(FrameEvent evt)
        {
            mOgreFactory.UpdateAgentsAction(evt);
        }
    }
}

