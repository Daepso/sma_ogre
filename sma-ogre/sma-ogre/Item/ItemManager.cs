using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sma_ogre.item
{
    class ItemManager
    {
        private List<Item> mItemList;

        public ItemManager()
        {
            mItemList = new List<Item>();
        }

        public void Add(Item i)
        {
            mItemList.Add(i);
        }

        public void Remove(Item i)
        {
            mItemList.Remove(i);
        }

        public List<Item> GetItemInSight(float x, float z, float sightRange)
        {
            List<Item> list = new List<Item>();
            foreach(Item i in mItemList)
            {
                float dis = i.Distance(x, z);
                if(dis < sightRange)
                {
                    list.Add(i);
                }
            }
            return list;
        }

    }
}
