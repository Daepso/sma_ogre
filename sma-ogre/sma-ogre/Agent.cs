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

        public Agent(SceneManager sceneMgr, string meshName, Vector3 meshFacedDirection, Behavior agentBehavior, Vector3 position,
            bool useAnimation = false)
        {
            mEntity     = sceneMgr.CreateEntity(meshName);
            mEntityNode = sceneMgr.RootSceneNode.CreateChildSceneNode();
            mEntityNode.AttachObject(mEntity);
            mEntityNode.Position = position;

            mAgentBehavior = agentBehavior;
            mAgentBehavior.Setup(mEntityNode, meshFacedDirection);
            mAgentBehavior.Init();

            mAgentAnimation = null;

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
