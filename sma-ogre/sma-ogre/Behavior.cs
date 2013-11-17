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

        public virtual void Update(float elapsedTime)
        {
            mAgentNode.Translate(direction * elapsedTime * speed);
        }
    }

    class BuilderBehavior : Behavior
    {
        static float disPickUp = 20;
        static float disDrop = 20;
        static float timeToWait = 3;

        private List<Item> listItem;
        private Item item;
        private float timerItem;

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


            if (timerItem <= 0)
            {
                if (item == null)
                {
                    foreach (Item i in listItem)
                    {
                        if (i.distance(mAgentNode.Position.x, mAgentNode.Position.z) < disPickUp)
                        {
                            Console.WriteLine("PickUp!" + i.distance(mAgentNode.Position.x, mAgentNode.Position.z));
                            item = i;
                            item.pickUp();
                            listItem.Remove(item);
                            timerItem = BuilderBehavior.timeToWait;
                            break;
                        }
                    }
                }
                else
                {
                    foreach (Item i in listItem)
                    {
                        if (i.distance(mAgentNode.Position.x, mAgentNode.Position.z) < disDrop)
                        {
                            Console.WriteLine("Drop!" + i.distance(mAgentNode.Position.x, mAgentNode.Position.z));
                            item.drop(mAgentNode.Position.x, mAgentNode.Position.z);
                            listItem.Add(item);
                            item = null;
                            timerItem = BuilderBehavior.timeToWait;
                            break;
                        }
                    }
                }
            }
            else
            {
                timerItem -= elapsedTime;
            }

        }

        public BuilderBehavior(List<Item> listItem)
        {
            this.listItem = listItem;
            item = null;
            timerItem = 0;
        }
    }
}
