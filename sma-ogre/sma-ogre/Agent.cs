using Mogre;
using System;

namespace sma_ogre
{
    class Agent
    {
        private Entity    mEntity;
        private SceneNode mEntityNode;
        private Behavior  mAgentBehavior;

        public Agent(SceneManager sceneMgr, string meshName)
        {
            mEntity     = sceneMgr.CreateEntity(meshName);
            mEntityNode = sceneMgr.RootSceneNode.CreateChildSceneNode();
            mEntityNode.AttachObject(mEntity);

            mAgentBehavior = new Behavior(mEntityNode);
        }

        public Agent(SceneManager sceneMgr, string meshName, Vector3 position) : this(sceneMgr, meshName)
        {
            mEntityNode.Position = position;
        }

        public void UpdateAction(FrameEvent evt)
        {
            mAgentBehavior.Update(evt.timeSinceLastFrame);
        }
    }
}
