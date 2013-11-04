using Mogre;

namespace sma_ogre
{
    class Agent
    {
        private Entity    mEntity;
        private SceneNode mEntityNode;

        public Agent(SceneManager sceneMgr, string meshName)
        {
            mEntity     = sceneMgr.CreateEntity(meshName);
            mEntityNode = sceneMgr.RootSceneNode.CreateChildSceneNode();
            mEntityNode.AttachObject(mEntity);
        }

        public Agent(SceneManager sceneMgr, string meshName, Vector3 position) : this(sceneMgr, meshName)
        {
			mEntityNode.Position = position;
        }
    }
}
