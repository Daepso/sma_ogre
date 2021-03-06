﻿using Mogre;
using sma_ogre.utils;

namespace sma_ogre.behavior
{
    class Behavior
    {
        protected SceneNode mAgentNode;
        protected Vector3   mMeshFacedDirection;
        protected Vector3   targetPosition;
        protected bool      isAtTargetPosition;
        protected float     speed;
        protected float     baseSpeed;

        public void Setup(SceneNode agentNode, Vector3 meshFacedDirection)
        {
            mAgentNode          = agentNode;
            mMeshFacedDirection = meshFacedDirection;
        }

        public virtual void Init()
        {
            targetPosition = new Vector3(0f, 0f, 0f);
            isAtTargetPosition = false;
            baseSpeed      = 0f;
            speed          = baseSpeed;
        }

        public virtual void ChooseRandomTargetPosition()
        {
            targetPosition.x = WorldConfig.Singleton.RandFloat(-WorldConfig.Singleton.GroundLength / 2f,
                                                                WorldConfig.Singleton.GroundLength / 2f);
            targetPosition.z = WorldConfig.Singleton.RandFloat(-WorldConfig.Singleton.GroundLength / 2f,
                                                                WorldConfig.Singleton.GroundLength / 2f);

            baseSpeed = WorldConfig.Singleton.RandFloat(WorldConfig.Singleton.DefaultSpeedRange[0],
                                                        WorldConfig.Singleton.DefaultSpeedRange[1]);
            isAtTargetPosition = false;
        }

        protected void FaceDirection(Vector3 direction)
        {
            Vector3 src = mAgentNode.Orientation * mMeshFacedDirection;
            // Rotate will fail if we ask for a 180 degrees rotation, we use Yaw in this particular case
            if ((1.0f + src.DotProduct(direction)) < 0.0001f)
            {
                mAgentNode.Yaw(180.0f);
            }
            else
            {
                Quaternion quat = src.GetRotationTo(direction);
                mAgentNode.Rotate(quat);
            }
        }

        protected void MoveToTargetPosition(float elapsedTime)
        {
            Vector3 direction = targetPosition - mAgentNode.Position;
            FaceDirection(direction);
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
                this.isAtTargetPosition = true;
   
            }    
        }

        public virtual void Update(float elapsedTime, AgentAnimation animation = null)
        {
            AdjustSpeed();
            MoveToTargetPosition(elapsedTime);
            if(isAtTargetPosition)
            {
                this.ChooseRandomTargetPosition();
            }

            if (animation != null)
            {
                animation.UpdatePosture(elapsedTime, speed);
            }
        }

        public void AdjustSpeed()
        {
            if (WorldTime.Singleton.SpeedFactor < 1)
            {
                speed = baseSpeed * WorldTime.Singleton.SpeedFactor;
            }
            else
            {
                speed = baseSpeed;
            }
        }

        public virtual void Die()
        {
        }
    }
}
