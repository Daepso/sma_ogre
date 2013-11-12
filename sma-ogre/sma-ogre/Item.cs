using Mogre;
using System;

namespace sma_ogre
{
    class Item
    {
        private Entity mEntity;
        private SceneNode mEntityNode;

        public Item(SceneManager sceneMgr, string meshName)
        {
            mEntity = sceneMgr.CreateEntity(meshName);
            mEntityNode = sceneMgr.RootSceneNode.CreateChildSceneNode();
            mEntityNode.AttachObject(mEntity);
        }

        public Item(SceneManager sceneMgr, string meshName, Vector3 pos):
            this(sceneMgr, meshName)        
        {
            mEntityNode.Position = pos;
        }


    }
}
