using Mogre;

namespace sma_ogre
{
    class AgentFactory
    {
        private SceneManager mSceneMgr;
        private string       mMeshName;

        private AgentFactory(SceneManager sceneMgr, string meshName)
        {
            mSceneMgr = sceneMgr;
			mMeshName = meshName;
        }

		public static AgentFactory OgreFactory(SceneManager sceneMgr)
        {
            return new AgentFactory(sceneMgr, "ogrehead.mesh");
        }

        public Agent makeAgent()
        {
            return new Agent(mSceneMgr, mMeshName);
        }

        public Agent makeAgent(bool useRandPos)
        {
			Vector3 pos = new Vector3(0, 0, 0);

			if (useRandPos) {
				System.Random rnd = new System.Random();
                pos.x = rnd.Next(-300, 300);
                pos.z = rnd.Next(-300, 300);
            }

            return new Agent(mSceneMgr, mMeshName, pos);
        }
    }
}
