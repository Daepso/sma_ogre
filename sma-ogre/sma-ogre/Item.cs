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

        public Item(SceneManager sceneMgr, string meshName, Vector3 pos) :
            this(sceneMgr, meshName)
        {
            mEntityNode.Position = pos;
        }

        public float Distance(float obj_x, float obj_z)
        {
            float x = (obj_x - mEntityNode.Position.x);
            float z = (obj_z - mEntityNode.Position.z);
            float dis = (float)System.Math.Sqrt(x * x + z * z);
            return dis;
        }

        public void PickUp()
        {
            mEntity.Visible = false;
        }

        public void Drop(float x, float z)
        {
            mEntityNode.SetPosition(x, mEntityNode.Position.y, z);
            mEntity.Visible = true;
        }
    }
}
