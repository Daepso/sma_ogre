using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sma_ogre.behavior
{
    class WreckerBehavior : Behavior
    {
        static float pickUpDistance = 20;
        static float dropDistance = 20;
  

        static float minCarriedTime = 2;
        static float maxCarriedTime = 10;

        private List<Item> listItem;
        private Item carriedItem;

        protected bool WreckeAction(float elapsedTime)
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
                            return true;
                            actionTimer = WreckerBehavior.actionDelay;
                            break;
                        }
                    }
                }
                else
                {
                    if(WorldConfig.Singleton.RandFloat(
                    foreach (Item i in listItem)
                    {
                        if (i.distance(mAgentNode.Position.x, mAgentNode.Position.z) < dropDistance)
                        {
                            carriedItem.drop(mAgentNode.Position.x, mAgentNode.Position.z);
                            listItem.Add(carriedItem);
                            carriedItem = null;
                            actionTimer = WreckerBehavior.actionDelay;
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
            WreckeAction(elapsedTime);
        }

        public WreckerBehavior(List<Item> listItem)
        {
            this.listItem = listItem;
            carriedItem = null;
            actionTimer = 0;
        }
    }
}
