using Mogre;
using System;

namespace sma_ogre
{
    class Program : OgreApp
    {
        AgentFactory mOgreFactory;
        ItemFactory mPenguinFactory;

        public static void Main()
        {
            try
            {
                new Program().Launch();
            }
            catch (OperationCanceledException) 
            { 
                Environment.Exit(0);
            }
        }

        protected override void CreateScene()
        {
            mSceneMgr.AmbientLight = new ColourValue(0.5f, 0.5f, 0.5f);

            CreateGround();

            mPenguinFactory = ItemFactory.penguinFactory(mSceneMgr);
            mPenguinFactory.makeNumItems(75, true);

            mOgreFactory = AgentFactory.OgreFactory(mSceneMgr,mPenguinFactory.itemsList());
            mOgreFactory.MakeNumAgents(WorldConfig.Singleton.InitialGoodAgentsNumber, true);
        }

        protected override void UpdateScene(FrameEvent evt)
        {
            mOgreFactory.UpdateAgentsAction(evt);
        }

        protected void CreateGround()
        {
            //TODO Add a table and a desk light because david said it's so cool.

            Plane plane = new Plane(Vector3.UNIT_Y, 0);
            MeshManager.Singleton.CreatePlane("ground",
                ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, plane,
                WorldConfig.Singleton.GroundWidth  + WorldConfig.Singleton.GroundBorderWidth,
                WorldConfig.Singleton.GroundLength + WorldConfig.Singleton.GroundBorderWidth,
                20, 20, true, 1, 5, 5, Vector3.UNIT_Z);

            Entity groundEnt = mSceneMgr.CreateEntity("GroundEntity", "ground");
            mSceneMgr.RootSceneNode.CreateChildSceneNode().AttachObject(groundEnt);

            groundEnt.SetMaterialName("Examples/Rockwall");
            groundEnt.CastShadows = false;
        }


    }
}

