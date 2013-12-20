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

        private bool mAgentIsDead;

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

            mAgentIsDead = false;
        }

        public void UpdateAction(FrameEvent evt)
        {
            if (AgentDies(evt.timeSinceLastFrame))
            {
                Kill();
                mAgentIsDead = true;
            }

            if (!mAgentIsDead)
            {
                mAgentBehavior.Update(evt.timeSinceLastFrame, mAgentAnimation);
            }
        }

        private bool AgentDies(float elapsedTime)
        {
            return WorldConfig.Singleton.RandFloat(0, 1) < AgentDeathProbability(elapsedTime);
        }

        private float AgentDeathProbability(float elapsedTime)
        {
            return 1.0f / 40 * elapsedTime;
        }

        public void Kill()
        {
            mAgentBehavior.Die();
        }

        public bool IsDead
        {
            get { return mAgentIsDead; }
        }
    }
}
