using Mogre;

namespace sma_ogre
{
    class World
    {
        static Light pointLightR;
        static Light pointLightB;
        static Light pointLightV;

        static public void Init(SceneManager sceneMgr)
        {
            sceneMgr.AmbientLight = WorldConfig.Singleton.AmbientLightOn;
            sceneMgr.SetSkyBox(true, "SpaceSkyBox");
            CreateGround(sceneMgr);
            CreateLights(sceneMgr);
        }

        static private void CreateGround(SceneManager sceneMgr)
        {
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

        static private void CreateLights(SceneManager sceneMgr)
        {
            pointLightR = sceneMgr.CreateLight("pointLightR");
            pointLightR.Type = Light.LightTypes.LT_POINT;
            pointLightR.Position = new Vector3(0, 150, 250);
            pointLightR.DiffuseColour = ColourValue.Red;
            pointLightR.SpecularColour = ColourValue.Red;

            pointLightB = sceneMgr.CreateLight("pointLightB");
            pointLightB.Type = Light.LightTypes.LT_POINT;
            pointLightB.Position = new Vector3(200, 150, -250);
            pointLightB.DiffuseColour = ColourValue.Blue;
            pointLightB.SpecularColour = ColourValue.Blue;

            

            pointLightV = sceneMgr.CreateLight("pointLightV");
            pointLightV.Type = Light.LightTypes.LT_POINT;
            pointLightV.Position = new Vector3(-200, 150, -250);
            pointLightV.DiffuseColour = ColourValue.Green;
            pointLightV.SpecularColour = ColourValue.Green;
        }

        static public void TurnOffOnLigths()
        {
            if (pointLightR.Visible == true)
            {
                pointLightR.Visible = false;
                pointLightB.Visible = false;
                pointLightV.Visible = false;
            }
            else
            {
                pointLightR.Visible = true;
                pointLightB.Visible = true;
                pointLightV.Visible = true;
            }
        }

    }
}
