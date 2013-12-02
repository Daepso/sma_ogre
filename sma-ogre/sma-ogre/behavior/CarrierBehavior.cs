using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sma_ogre.behavior
{
    class CarrierBehavior : Behavior
    {
        protected List<Item> listItem;
        protected Item carriedItem;

        public CarrierBehavior(List<Item> listItem)
        {
            this.listItem = listItem;
            carriedItem = null;
        }

        protected void pickUpAction(Item i)
        {
              carriedItem = i;
              carriedItem.PickUp(mAgentNode);
              listItem.Remove(carriedItem);
        }

        protected void dropAction(float x, float z)
        {
            carriedItem.Drop(x, z, mAgentNode);
            listItem.Add(carriedItem);
            carriedItem = null;
        }
    }
}
