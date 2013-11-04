using System;
using Mogre;

namespace sma_ogre
{
    class Behavior
    {
        private static int ID = 0;
        private SceneNode  mAgentNode;
        private Random     rnd;
        private Vector3    direction;
        private int        speed;

        public Behavior(SceneNode agentNode)
        {
            mAgentNode = agentNode;

            // Ensure that every number generator is different
            // If this is not done, diverse behaviors can be similar
            rnd = new Random(ID++);

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
}
