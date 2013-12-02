using Mogre;
using System;

namespace sma_ogre
{
    class Item
    {
        private Entity mEntity;
        private SceneNode mEntityNode;
        private SceneManager sceneMgr;

        public Item(SceneManager sceneMgr, string meshName)
        {
            this.sceneMgr = sceneMgr;
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

        public void PickUp(SceneNode mAgentNode)
        {
            sceneMgr.RootSceneNode.RemoveChild(mEntityNode);
            mAgentNode.AddChild(mEntityNode);
            mEntityNode.SetPosition(0, mEntityNode.Position.y + 80, 0);
        }

        public void Drop(float x, float z, SceneNode mAgentNode)
        {       
            mAgentNode.RemoveChild(mEntityNode);
            sceneMgr.RootSceneNode.AddChild(mEntityNode);
            mEntityNode.SetPosition(x, mEntityNode.Position.y - 80, z);
        }
    }
}
