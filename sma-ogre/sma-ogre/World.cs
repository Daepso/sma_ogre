using Mogre;

namespace sma_ogre
{
    class World
    {

        static public void init(SceneManager sceneMgr)
        {
            sceneMgr.AmbientLight = WorldConfig.Singleton.AmbientLightOn;
            sceneMgr.SetSkyBox(true, "SpaceSkyBox");
            CreateGround(sceneMgr);
        }

        static private void CreateGround(SceneManager sceneMgr)
        {
            //TODO Add a table and a desk light because david said it's so cool.

            Plane plane = new Plane(Vector3.UNIT_Y, 0);
            MeshManager.Singleton.CreatePlane("ground",
                ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, plane,
                WorldConfig.Singleton.GroundWidth + WorldConfig.Singleton.GroundBorderWidth,
                WorldConfig.Singleton.GroundLength + WorldConfig.Singleton.GroundBorderWidth,
                20, 20, true, 1, 5, 5, Vector3.UNIT_Z);

            Entity groundEnt = sceneMgr.CreateEntity("GroundEntity", "ground");
            sceneMgr.RootSceneNode.CreateChildSceneNode().AttachObject(groundEnt);

            groundEnt.SetMaterialName("Examples/Rockwall");
            groundEnt.CastShadows = false;
        }
    }
}
