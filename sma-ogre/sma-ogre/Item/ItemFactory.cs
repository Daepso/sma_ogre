using Mogre;
using System.Collections.Generic;

namespace sma_ogre.item
{
    class ItemFactory
    {
        private SceneManager mSceneMgr;
        private string       mMeshName;
        private ItemManager mItemManager;


        private ItemFactory(SceneManager sceneMgr, string meshName)
        {
            mSceneMgr = sceneMgr;
            mMeshName = meshName;
            mItemManager = new ItemManager();
        }

        public static ItemFactory BrickFactory(SceneManager sceneMgr)
        {
            return new ItemFactory(sceneMgr, WorldConfig.Singleton.BrickMesh);
        }

        /* Add a new Resource to the world at a random position*/
        public Item MakeItem(bool useRandPos = true)
        {
            Vector3 pos = new Vector3(0, 30, 0);

            if (useRandPos)
            {
                pos.x = WorldConfig.Singleton.RandFloat(
                    -WorldConfig.Singleton.GroundWidth / 2,
                     WorldConfig.Singleton.GroundWidth / 2);
                pos.z = WorldConfig.Singleton.RandFloat(
                    -WorldConfig.Singleton.GroundLength / 2,
                     WorldConfig.Singleton.GroundLength / 2);
            }

            Item item = new Item(mSceneMgr, mMeshName, pos);
            mItemManager.Add(item);

            return item;
        }

        /*Add resources to the world at random positions*/
        public void MakeNumItems(int itemNumber, bool useRandPos = true)
        {
            for (int i = 0; i < itemNumber; i++)
            {
                MakeItem(useRandPos);
            }
        }

        public ItemManager getItemManager()
        {
            return mItemManager;
        }
    }
}
