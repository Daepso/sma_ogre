using Mogre;
using System;

namespace sma_ogre
{
    class Agent
    {
        private Entity    mEntity;
        private SceneNode mEntityNode;
        private Behavior  mAgentBehavior;

        public Agent(SceneManager sceneMgr, string meshName, Behavior agentBehavior)
        {
            mEntity     = sceneMgr.CreateEntity(meshName);
            mEntityNode = sceneMgr.RootSceneNode.CreateChildSceneNode();
            mEntityNode.AttachObject(mEntity);

            mAgentBehavior = agentBehavior;
            mAgentBehavior.Setup(mEntityNode);
            mAgentBehavior.Init();
        }

        public Agent(SceneManager sceneMgr, string meshName, Behavior agentBehavior, Vector3 position):
            this(sceneMgr, meshName, agentBehavior)
        {
            mEntityNode.Position = position;
        }

        public void UpdateAction(FrameEvent evt)
        {
            mAgentBehavior.Update(evt.timeSinceLastFrame);
        }
    }
}
