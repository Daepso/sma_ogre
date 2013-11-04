using Mogre;
using System;

namespace sma_ogre
{
    class Agent
    {
        private Entity    mEntity;
        private SceneNode mEntityNode;
        private Random rnd;
        private Vector3 move;
        int speed;


        public Agent(SceneManager sceneMgr, string meshName)
        {
            mEntity     = sceneMgr.CreateEntity(meshName);
            mEntityNode = sceneMgr.RootSceneNode.CreateChildSceneNode();
            mEntityNode.AttachObject(mEntity);
            rnd = new Random();

            move = new Vector3(0, 0, 0);
            move.x = (float)rnd.NextDouble();
            move.z = (float)rnd.NextDouble();

            speed = rnd.Next(100);
        }

        public Agent(SceneManager sceneMgr, string meshName, Vector3 position) : this(sceneMgr, meshName)
        {
			mEntityNode.Position = position;
        }

        public void React(FrameEvent evt)
        {
            mEntityNode.Translate(move * evt.timeSinceLastFrame * speed);
        }
    }
}
