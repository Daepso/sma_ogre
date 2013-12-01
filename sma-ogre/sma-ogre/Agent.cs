using Mogre;
using System;
using sma_ogre.behavior;

namespace sma_ogre
{
    class Agent
    {
        private Entity         mEntity;
        private SceneNode      mEntityNode;
        private Behavior       mAgentBehavior;
        private AgentAnimation mAgentAnimation;

        public Agent(SceneManager sceneMgr, string meshName, Behavior agentBehavior)
        {
            mEntity     = sceneMgr.CreateEntity(meshName);
            mEntityNode = sceneMgr.RootSceneNode.CreateChildSceneNode();
            mEntityNode.AttachObject(mEntity);

            mAgentBehavior = agentBehavior;
            mAgentBehavior.Setup(mEntityNode);
            mAgentBehavior.Init();

            mAgentAnimation = null;
        }

        public Agent(SceneManager sceneMgr, string meshName, Behavior agentBehavior, Vector3 position, bool useAnimation = false):
            this(sceneMgr, meshName, agentBehavior)
        {
            mEntityNode.Position = position;

            if (useAnimation)
            {
                mAgentAnimation = new AgentAnimation(mEntity);
            }
        }

        public void UpdateAction(FrameEvent evt)
        {
            mAgentBehavior.Update(evt.timeSinceLastFrame, mAgentAnimation);
        }
    }
}
