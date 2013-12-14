using System.Collections.Generic;
using sma_ogre.item;

namespace sma_ogre.behavior
{
    class CarrierBehavior : Behavior
    {
        static protected float actionDistance = 20;

        protected ItemManager itemManager;
        protected Item       carriedItem;

        public override void Init()
        {
            base.Init();
            this.ChooseRandomTargetPosition();
        }

        public CarrierBehavior(ItemManager itemManager)
        {
            this.itemManager = itemManager;
            carriedItem   = null;
        }

        protected void PickUpAction(Item i)
        {
              carriedItem = i;
              carriedItem.PickUp(mAgentNode);
              itemManager.Remove(carriedItem);
        }

        protected void DropAction(float x, float z)
        {
            carriedItem.Drop(x, z, mAgentNode);
            itemManager.Add(carriedItem);
            carriedItem = null;
        }

        protected bool SearchObjectAction()
        {
            List<Item> listItem = itemManager.GetItemInSight(mAgentNode.Position.x,
                                                             mAgentNode.Position.z,
                                                             WorldConfig.Singleton.BuilderSightRange);
            float minDis = float.MaxValue;
            Item closestItem = null;
            foreach (Item i in listItem)
            {
                float dis = i.Distance(mAgentNode.Position.x, mAgentNode.Position.z);
                if (dis < minDis)
                {
                    if (dis < actionDistance)
                    {
                        PickUpAction(i);
                        return true;
                    }
                    else
                    {
                        minDis = dis;
                        closestItem = i;
                    }
                }
            }

            if (closestItem != null)
            {
                targetPosition.x = closestItem.getPositionX();
                targetPosition.z = closestItem.getPositionZ();
            }
            return false;
        }
    }
}
