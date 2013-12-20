using Mogre;
using System;
using sma_ogre.behavior;
using sma_ogre.utils;

namespace sma_ogre
{
    class Agent
    {
        private Entity         mEntity;
        private SceneNode      mEntityNode;
        private Behavior       mAgentBehavior;
        private AgentAnimation mAgentAnimation;

        private bool mAgentIsMortal;
        private bool mAgentIsDead;

        public Agent(SceneManager sceneMgr, string meshName, Vector3 meshFacedDirection, Behavior agentBehavior, Vector3 position,
            bool useAnimation = false, bool isMortal = true)
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

            mAgentIsMortal = isMortal;
            mAgentIsDead = false;
        }

        public void UpdateAction(FrameEvent evt)
        {
            if (mAgentIsMortal && AgentDies(evt.timeSinceLastFrame))
            {
                Kill();
                mAgentIsDead = true;
            }

            if (!mAgentIsMortal || !mAgentIsDead)
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
            float ln2 = 0.69314718f;
            return ln2 / WorldConfig.Singleton.AgentsHalfLife * elapsedTime;
        }

        public void Kill()
        {
            mAgentBehavior.Die();
            InformationLogger.Singleton.NewDeath();
            mEntityNode.DetachAllObjects();
        }

        public bool IsDead
        {
            get { return mAgentIsDead; }
        }

        public SceneNode GetNode()
        {
            return mEntityNode; 
        }
    }
}
