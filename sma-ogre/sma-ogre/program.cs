using Mogre;
using System;
using sma_ogre.item;

namespace sma_ogre
{
    class Program : OgreApp
    {
        AgentFactory mOgreFactory;
        AgentFactory mRobotFactory;
        ItemFactory  mBrickFactory;

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
            World.init(mSceneMgr);

            mBrickFactory = ItemFactory.BrickFactory(mSceneMgr);
            mBrickFactory.MakeNumItems(WorldConfig.Singleton.InitialBrickNumber, true);

            mOgreFactory  = AgentFactory.OgreFactory(mSceneMgr,mBrickFactory.getItemManager());
            mOgreFactory.MakeNumAgents(WorldConfig.Singleton.InitialGoodAgentsNumber, true);

            mRobotFactory = AgentFactory.RobotFactory(mSceneMgr,mBrickFactory.getItemManager());
            mRobotFactory.MakeNumAgents(WorldConfig.Singleton.InitialBadAgentsNumber, true, true);
        }

        protected override void UpdateScene(FrameEvent evt)
        {
            if (WorldConfig.Singleton.Pause)
            {
                mOgreFactory.UpdateAgentsAction(evt);
                mRobotFactory.UpdateAgentsAction(evt);
            }
        }

  
    }
}