using Mogre;
using System.Collections.Generic;

namespace sma_ogre
{
    class AgentFactory
    {
        private SceneManager mSceneMgr;
        private string       mMeshName;
        private List<Agent>  mAgents;

        private AgentFactory(SceneManager sceneMgr, string meshName)
        {
            mSceneMgr = sceneMgr;
			mMeshName = meshName;
            mAgents = new List<Agent>();
        }

		public static AgentFactory OgreFactory(SceneManager sceneMgr)
        {
            return new AgentFactory(sceneMgr, "ogrehead.mesh");
        }

        public Agent MakeAgent()
        {
            return MakeAgent(false);
        }

        public Agent MakeAgent(bool useRandPos)
        {
            Vector3 pos = new Vector3(0, 0, 0);

            if (useRandPos)
            {
                System.Random rnd = new System.Random();
                pos.x = rnd.Next(-100, 100);
                pos.z = rnd.Next(-300, 300);
            }

            Agent agent = new Agent(mSceneMgr, mMeshName, pos);
            mAgents.Add(agent);

            return agent;
        }

        public void UpdateAgentsAction(FrameEvent evt)
        {
            foreach (Agent agent in mAgents)
            {
                agent.React(evt);
            }
        }
    }
}
