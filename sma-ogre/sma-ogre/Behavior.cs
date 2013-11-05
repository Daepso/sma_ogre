using System;
using Mogre;

namespace sma_ogre
{
    class Behavior
    {
        private   static int ID = 0;
        protected SceneNode  mAgentNode;
        protected Random     rnd;
        protected Vector3    direction;
        protected int        speed;

        public void Setup(SceneNode agentNode)
        {
            mAgentNode = agentNode;

            // Ensure that every number generator is different
            // If this is not done, diverse behaviors can be similar
            rnd = new Random(ID++);
        }

        public virtual void Init()
        {
            direction   = new Vector3(0, 0, 0);
            direction.x = (float)rnd.NextDouble() - 0.5f;
            direction.z = (float)rnd.NextDouble() - 0.5f;

            speed = rnd.Next(10, 100);
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

            speed = rnd.Next(100, 1000);
        }

        public override void Update(float elapsedTime)
        {
            if (mAgentNode.Position.x >= 750 || mAgentNode.Position.x <= -750)
            {
                direction.x *= -1;
            }

            if (mAgentNode.Position.z >= 750 || mAgentNode.Position.z <= -750)
            {
                direction.z *= -1;
            }

            mAgentNode.Translate(direction * elapsedTime * speed);
        }
    }
}
