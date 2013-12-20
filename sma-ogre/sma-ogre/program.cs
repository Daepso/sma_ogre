using Mogre;
using System;
using sma_ogre.item;
using sma_ogre.utils;

namespace sma_ogre
{
    class Program : OgreApp
    {
        private AgentFactory mOgreFactory;
        private AgentFactory mRobotFactory;
        private ItemFactory  mBrickFactory;

        private float frameToPlay;

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
            World.Init(mSceneMgr);

            mBrickFactory = ItemFactory.BrickFactory(mSceneMgr);
            mBrickFactory.MakeNumItems(WorldConfig.Singleton.InitialBrickNumber, true);

            mOgreFactory  = AgentFactory.OgreFactory(mSceneMgr, mBrickFactory.GetItemManager());
            mOgreFactory.MakeNumAgents(WorldConfig.Singleton.InitialGoodAgentsNumber, true);

            mRobotFactory = AgentFactory.RobotFactory(mSceneMgr, mBrickFactory.GetItemManager());
            mRobotFactory.MakeNumAgents(WorldConfig.Singleton.InitialBadAgentsNumber, true, true, false);

            OverlayUtils.Singleton.Init(mRenderWindow);

            frameToPlay = 1;
        }

        protected override void UpdateScene(FrameEvent evt)
        {
            frameToPlay = WorldTime.Singleton.SpeedFactor;

            while (frameToPlay >= 2)
            {
                UpdateFrame(evt, evt.timeSinceLastFrame);
                frameToPlay--;
            }
            UpdateFrame(evt, evt.timeSinceLastFrame*frameToPlay);
        }

        protected void UpdateFrame(FrameEvent evt, float worldElapsedTime)
        {
            if (WorldTime.Singleton.Pause)
            {
                mOgreFactory.UpdateAgentsAction(evt);
                mRobotFactory.UpdateAgentsAction(evt);

                WorldTime.Singleton.UpdateTime(worldElapsedTime);
                LogInformation();
            }
            OverlayUtils.Singleton.Update();
        }

        protected void LogInformation()
        {
            InformationLogger.Singleton.OgresNumber  = mOgreFactory.GetAgentsNumber();
            InformationLogger.Singleton.RobotsNumber = mRobotFactory.GetAgentsNumber();
        }
    }
}
