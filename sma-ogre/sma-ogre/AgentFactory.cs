using Mogre;
using System.Collections.Generic;
using sma_ogre.behavior;
using sma_ogre.item;

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

        public static AgentFactory OgreFactory(SceneManager sceneMgr, ItemManager itemManager)
        {
            if (WorldConfig.Singleton.IsBuilderClever)
            {
                return new AgentFactory(sceneMgr, WorldConfig.Singleton.OgreMesh, new CleverBuilderBehaviorFactory(itemManager));
                
            }
            else
            {
                return new AgentFactory(sceneMgr, WorldConfig.Singleton.OgreMesh, new BuilderBehaviorFactory(itemManager));
            }
        }

        public static AgentFactory RobotFactory(SceneManager sceneMgr, ItemManager itemManager)
        {
			AgentFactory agentFactory =
                new AgentFactory(sceneMgr, WorldConfig.Singleton.RobotMesh, new WreckerBehaviorFactory(itemManager));
            agentFactory.CorrectMeshFacedDirection(Vector3.UNIT_X);
            return agentFactory;
        }

        public Agent MakeAgent(bool useRandPos = false, bool useAnimation = false, bool isMortal = true)
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

            Agent agent = new Agent(mSceneMgr, mMeshName, mMeshFacedDirection, mBehaviorFactory.MakeBehavior(), pos, useAnimation, isMortal);
            mAgents.Add(agent);

            return agent;
        }

        public void MakeNumAgents(int agentNumber, bool useRandPos = false, bool useAnimation = false, bool isMortal = true)
        {
            for (int i = 0; i < agentNumber; i++)
            {
                MakeAgent(useRandPos, useAnimation, isMortal);
            }
        }

        public int GetAgentsNumber()
        {
            return mAgents.Count;
        }

        public void UpdateAgentsAction(FrameEvent evt)
        {
            List<Agent> deadAgents = new List<Agent>();
            foreach (Agent agent in mAgents)
            {
                if (agent.IsDead)
                {
                    deadAgents.Add(agent);
                }
                else
                {
                    agent.UpdateAction(evt);
                }
            }

            // Remove dead agents
            foreach (Agent agent in deadAgents)
            {
                mAgents.Remove(agent);
            }
        }

        public SceneNode GetRandomNodeAgent()
        {
            if(mAgents.Count != 0)
            {
                int randId = WorldConfig.Singleton.RandInt(0,mAgents.Count-1);
                return mAgents[randId].GetNode();
            }
            else
                return null;
        }
    }
}
