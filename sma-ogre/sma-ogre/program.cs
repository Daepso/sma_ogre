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

            Agent ogre  = mOgreFactory.MakeAgent(true);
            //Agent ogre2 = mOgreFactory.MakeAgent(true );
            //Agent ogre3 = mOgreFactory.MakeAgent(true );
            //Agent ogre4 = mOgreFactory.MakeAgent(true );
            //Agent ogre5 = mOgreFactory.MakeAgent(true );
            //Agent ogre6 = mOgreFactory.MakeAgent(true );
        }

        protected override void UpdateScene(FrameEvent evt)
        {
            mOgreFactory.UpdateAgentsAction(evt);
        }
    }
}

