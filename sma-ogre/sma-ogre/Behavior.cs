using System;
using Mogre;

namespace sma_ogre
{
    class Behavior
    {
        private   static int ID = 0;
        protected SceneNode  mAgentNode;
        protected Vector3    direction;
        protected float      speed;

        public void Setup(SceneNode agentNode)
        {
            mAgentNode = agentNode;
        }

        public virtual void Init()
        {
            direction   = new Vector3(0f, 0f, 0f);
            direction.x = WorldConfig.Singleton.RandFloat(-1f, 1f);
            direction.z = WorldConfig.Singleton.RandFloat(-1f, 1f);

            speed = WorldConfig.Singleton.RandFloat(10, 100);
        }

        public virtual void Update(float elapsedTime)
        {
            mAgentNode.Translate(direction * elapsedTime * speed);
        }
    }

    class BuilderBehavior : Behavior
    {
        public override void Init()
        {
            base.Init();

            speed = WorldConfig.Singleton.RandFloat(100, 1000);
        }

        public override void Update(float elapsedTime)
        {
            if ( mAgentNode.Position.x >=  WorldConfig.Singleton.GroundWidth / 2 ||
                 mAgentNode.Position.x <= -WorldConfig.Singleton.GroundWidth / 2)
            {
                direction.x *= -1;
            }

            if ( mAgentNode.Position.z >=  WorldConfig.Singleton.GroundLength / 2 ||
                 mAgentNode.Position.z <= -WorldConfig.Singleton.GroundLength / 2)
            {
                direction.z *= -1;
            }

            mAgentNode.Translate(direction * elapsedTime * speed);
        }
    }
}
