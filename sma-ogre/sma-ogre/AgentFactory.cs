using Mogre;
using System.Collections.Generic;
using sma_ogre.behavior;

namespace sma_ogre
{
    class AgentFactory
    {
        private SceneManager    mSceneMgr;
        private Vector3         mMeshFacedDirection;
        private string          mMeshName;
        private BehaviorFactory mBehaviorFactory;
        private List<Agent>     mAgents;

        private AgentFactory(SceneManager sceneMgr, string meshName, BehaviorFactory agentsBehavior)
        {
            mSceneMgr           = sceneMgr;
            mMeshName           = meshName;
            mMeshFacedDirection = Vector3.UNIT_Z;
            mBehaviorFactory    = agentsBehavior;
            mAgents             = new List<Agent>();
        }

        private void CorrectMeshFacedDirection(Vector3 meshFacedDirection)
        {
            mMeshFacedDirection = meshFacedDirection;
        }

        public static AgentFactory OgreFactory(SceneManager sceneMgr, List<Item> listItem)
        {
            return new AgentFactory(sceneMgr, WorldConfig.Singleton.OgreMesh, new BuilderBehaviorFactory(listItem));
        }

        public static AgentFactory RobotFactory(SceneManager sceneMgr, List<Item> listItem)
        {
			AgentFactory agentFactory =
                new AgentFactory(sceneMgr, WorldConfig.Singleton.RobotMesh, new WreckerBehaviorFactory(listItem));
            agentFactory.CorrectMeshFacedDirection(Vector3.UNIT_X);
            return agentFactory;
        }

        public Agent MakeAgent(bool useRandPos = false, bool useAnimation = false)
        {
            Vector3 pos = new Vector3(0, 0, 0);

            if (useRandPos)
            {
                pos.x = WorldConfig.Singleton.RandFloat(
                    1-WorldConfig.Singleton.GroundWidth / 2,
                    1+WorldConfig.Singleton.GroundWidth / 2);
                pos.z = WorldConfig.Singleton.RandFloat(
                    1-WorldConfig.Singleton.GroundLength / 2,
                    1+WorldConfig.Singleton.GroundLength / 2);
            }

            Agent agent = new Agent(mSceneMgr, mMeshName, mMeshFacedDirection, mBehaviorFactory.MakeBehavior(), pos, useAnimation);
            mAgents.Add(agent);

            return agent;
        }

        public void MakeNumAgents(int agentNumber, bool useRandPos = false, bool useAnimation = false)
        {
            for (int i = 0; i < agentNumber; i++)
            {
                MakeAgent(useRandPos, useAnimation);
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
