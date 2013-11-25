using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sma_ogre.behavior
{
    class BuilderBehavior : Behavior
    {
        static float pickUpDistance = 20;
        static float dropDistance = 20;

        private List<Item> listItem;
        private Item carriedItem;
        private float actionTimer;

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
