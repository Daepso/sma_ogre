using System;
using Mogre;
using System.Collections.Generic;

namespace sma_ogre.behavior
{
    class Behavior
    {
        protected SceneNode  mAgentNode;
        protected Vector3   targetPosition;
        protected float      speed;

        public void Setup(SceneNode agentNode)
        {
            mAgentNode = agentNode;
        }

        public virtual void Init()
        {
            targetPosition = new Vector3(0f, 0f, 0f);
            speed = 0f;
        }

        public virtual void ChooseTargetPosition()
        {
            targetPosition.x = WorldConfig.Singleton.RandFloat(-WorldConfig.Singleton.GroundLength / 2f, WorldConfig.Singleton.GroundLength / 2f);
            targetPosition.z = WorldConfig.Singleton.RandFloat(-WorldConfig.Singleton.GroundLength / 2f, WorldConfig.Singleton.GroundLength / 2f);
            speed = WorldConfig.Singleton.RandFloat(200, 1000);
        }

        protected void MoveToTargetPosition(float elapsedTime)
        {
            Vector3 direction = targetPosition - mAgentNode.Position;
            Vector3 newPosition = new Vector3();
            newPosition += direction;
            newPosition.Normalise();
            newPosition *= speed * elapsedTime;
            if (newPosition.Length < direction.Length)
            {
                mAgentNode.Translate(newPosition);
            }
            else
            {
                mAgentNode.Translate(direction);
                this.ChooseTargetPosition();
            }    
        }

        public virtual void Update(float elapsedTime)
        {
            MoveToTargetPosition(elapsedTime);
        }
    }
}
