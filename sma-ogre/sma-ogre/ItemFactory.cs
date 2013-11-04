using Mogre;
using System.Collections.Generic;

namespace sma_ogre
{
    class ItemFactory
    {
        private SceneManager mSceneMgr;
        private string mMeshName;
        List<Item> itemList;

         private ItemFactory(SceneManager sceneMgr, string meshName)
        {
            mSceneMgr = sceneMgr;
			mMeshName = meshName;
        }

         public static ItemFactory BoxFactory(SceneManager sceneMgr)
         {
             //Todo Change the ogrehead.mesh with another mesh
             return new ItemFactory(sceneMgr, "ogrehead.mesh");
         }

        /* Add a new Resource to the world at a random position*/
         public void makeResource()
         {
         }

           /*Add a new Resource to the world at a given position*/
         public void makeResource(int x, int z)
         {
         }

    }
}
