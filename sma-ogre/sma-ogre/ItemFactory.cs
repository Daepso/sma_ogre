using Mogre;
using System.Collections.Generic;

namespace sma_ogre
{
    class ItemFactory
    {
        private SceneManager mSceneMgr;
        private string mMeshName;
        private List<Item> mItems;

         private ItemFactory(SceneManager sceneMgr, string meshName)
        {
            mSceneMgr = sceneMgr;
			mMeshName = meshName;
            mItems = new List<Item>();
        }

         public static ItemFactory brickFactory(SceneManager sceneMgr)
         {
             //Todo Change the ogrehead.mesh with another mesh
             return new ItemFactory(sceneMgr, "Cube.mesh");
         }

        /* Add a new Resource to the world at a random position*/
         public Item makeItem(bool useRandPos = true)
         {
             Vector3 pos = new Vector3(0, 30, 0);

             if(useRandPos)
             {
                pos.x = WorldConfig.Singleton.RandFloat(
                    -WorldConfig.Singleton.GroundWidth / 2,
                     WorldConfig.Singleton.GroundWidth / 2);
                pos.z = WorldConfig.Singleton.RandFloat(
                    -WorldConfig.Singleton.GroundLength / 2,
                     WorldConfig.Singleton.GroundLength / 2);
             }

             Item item = new Item(mSceneMgr, mMeshName, pos);
             mItems.Add(item);

             return item;
         }

           /*Add a new Resource to the world at a given position*/
         public void makeNumItems(int itemNumber, bool useRandPos = true)
         {
             for(int i = 0; i < itemNumber; i++)
             {
                 makeItem(useRandPos);
             }
         }

         public List<Item> itemsList()
         {
             return mItems;
         }

    }
}
