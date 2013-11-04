using Mogre;
using System;

namespace sma_ogre
{
    class Program : OgreApp
    {
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

            CreateTerrain();

            AgentFactory ogreFactory = AgentFactory.OgreFactory(mSceneMgr);

            Agent ogre  = ogreFactory.makeAgent(false);
            Agent ogre2 = ogreFactory.makeAgent(true );
            Agent ogre3 = ogreFactory.makeAgent(true );
        }

        protected override void UpdateScene(FrameEvent evt)
        {
        }

        protected void CreateTerrain()
        {
            //TODO Add a table and a desk light because david said it's so cool. 

            Plane plane = new Plane(Vector3.UNIT_Y, 0);
            MeshManager.Singleton.CreatePlane("ground", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME,
    plane, 1500, 1500, 20, 20, true, 1, 5, 5, Vector3.UNIT_Z);

            Entity groundEnt = mSceneMgr.CreateEntity("GroundEntity", "ground");
            mSceneMgr.RootSceneNode.CreateChildSceneNode().AttachObject(groundEnt);

            groundEnt.SetMaterialName("Examples/Rockwall");
            groundEnt.CastShadows = false;
        }


    }
}

