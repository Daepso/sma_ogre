using System;
using Mogre;
using System.Collections.Generic;

namespace sma_ogre
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
            this.ChooseTargetPosition();
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
            MoveToTargetPosition(elapsedTime);
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
