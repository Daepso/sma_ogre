using System.Collections.Generic;
using sma_ogre.item;

namespace sma_ogre.behavior
{
    class CarrierBehavior : Behavior
    {
        protected ItemManager itemManager;
        protected Item       carriedItem;

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
    }
}
