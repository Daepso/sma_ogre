using Mogre;
using System.Collections.Generic;

namespace sma_ogre
{
    class AgentFactory
    {
        private SceneManager    mSceneMgr;
        private string          mMeshName;
        private BehaviorFactory mBehaviorFactory;
        private List<Agent>     mAgents;

        private AgentFactory(SceneManager sceneMgr, string meshName, BehaviorFactory agentsBehavior)
        {
            mSceneMgr        = sceneMgr;
            mMeshName        = meshName;
            mBehaviorFactory = agentsBehavior;
            mAgents          = new List<Agent>();
        }

        public static AgentFactory OgreFactory(SceneManager sceneMgr, List<Item> listItem)
        {
            return new AgentFactory(sceneMgr, "ogrehead.mesh", new BuilderBehaviorFactory(listItem));
        }

        public static AgentFactory RobotFactory(SceneManager sceneMgr, List<Item> listItem)
        {
            return new AgentFactory(sceneMgr, "robot.mesh", new BuilderBehaviorFactory(listItem));
        }

        public Agent MakeAgent(bool useRandPos = false)
        {
            Vector3 pos = new Vector3(0, 30, 0);

            if (useRandPos)
            {
                pos.x = WorldConfig.Singleton.RandFloat(
                    1-WorldConfig.Singleton.GroundWidth / 2,
                    1+WorldConfig.Singleton.GroundWidth / 2);
                pos.z = WorldConfig.Singleton.RandFloat(
                    1-WorldConfig.Singleton.GroundLength / 2,
                    1+WorldConfig.Singleton.GroundLength / 2);
            }

            Agent agent = new Agent(mSceneMgr, mMeshName, mBehaviorFactory.MakeBehavior(), pos);
            mAgents.Add(agent);

            return agent;
        }

        public void MakeNumAgents(int agentNumber, bool useRandPos = false)
        {
            for (int i = 0; i < agentNumber; i++)
            {
                MakeAgent(useRandPos);
            }
        }

        public void UpdateAgentsAction(FrameEvent evt)
        {
            foreach (Agent agent in mAgents)
            {
                agent.UpdateAction(evt);
            }
        }
    }
}
