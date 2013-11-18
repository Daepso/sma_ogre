using System;
using Mogre;
using System.Collections.Generic;

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
            direction.Normalise();

            speed = WorldConfig.Singleton.RandFloat(10, 100);
        }

        protected void MoveWithCollision(Vector3 translation, float elapsedTime)
        {

            Vector3 newPosition = translation + mAgentNode.Position;

            //Console.WriteLine("newPos : " + newPosition);
            //Console.WriteLine("trans : " + translation);
            //Console.WriteLine("pos : " + mAgentNode.Position

            if (newPosition.x >=  WorldConfig.Singleton.GroundWidth / 2 ||
                newPosition.x <= -WorldConfig.Singleton.GroundWidth / 2)
            {
                direction.x *= -1;
            }

            if (newPosition.z >=  WorldConfig.Singleton.GroundLength / 2 ||
                newPosition.z <= -WorldConfig.Singleton.GroundLength / 2)
            {
                direction.z *= -1;
            }
            mAgentNode.Translate(direction * elapsedTime * speed);
        }

        public virtual void Update(float elapsedTime)
        {
            MoveWithCollision(direction,elapsedTime);
        }
    }

    class BuilderBehavior : Behavior
    {
        static float pickUpDistance = 20;
        static float dropDistance = 20;
        static float actionDelay = 3;

        private List<Item> listItem;
        private Item carriedItem;
        private float actionTimer ;

        public override void Init()
        {
            base.Init();
            speed = WorldConfig.Singleton.RandFloat(100, 1000);
        }

        protected void BuildAction(float elapsedTime)
        {
            if (actionTimer <= 0)
            {
                if (carriedItem == null)
                {
                    foreach (Item i in listItem)
                    {
                        if (i.distance(mAgentNode.Position.x, mAgentNode.Position.z) < pickUpDistance)
                        {                        
                            carriedItem = i;
                            carriedItem.pickUp();
                            listItem.Remove(carriedItem);
                            actionTimer = BuilderBehavior.actionDelay;
                            break;
                        }
                    }
                }
                else
                {
                    foreach (Item i in listItem)
                    {
                        if (i.distance(mAgentNode.Position.x, mAgentNode.Position.z) < dropDistance)
                        {                         
                            carriedItem.drop(mAgentNode.Position.x, mAgentNode.Position.z);
                            listItem.Add(carriedItem);
                            carriedItem = null;
                            actionTimer = BuilderBehavior.actionDelay;
                            break;
                        }
                    }
                }
            }
            else
            {
                actionTimer -= elapsedTime;
            }
        }


        public override void Update(float elapsedTime)
        {
            MoveWithCollision(direction, elapsedTime);
            BuildAction(elapsedTime);        
        }

        public BuilderBehavior(List<Item> listItem)
        {
            this.listItem = listItem;
            carriedItem = null;
            actionTimer = 0;
        }
    }
}
