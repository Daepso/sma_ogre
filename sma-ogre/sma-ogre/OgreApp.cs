using Mogre;
using System;

namespace sma_ogre
{
    partial class OgreApp
    {
        protected Root              mRoot;
        protected SceneManager      mSceneMgr;
        protected RenderWindow      mRenderWindow;
        protected Camera            mCamera;
        protected CameraRTS         mCameraRTS;

        public void Launch()
        {
            CreateRoot();
            DefineResources();
            CreateRenderSystem();
            CreateRenderWindow();
            InitializeResources();
            InitializeInput();

            ChooseSceneManager();
            CreateCamera();
            CreateViewports();
            CreateScene();

            CreateFrameListeners();
            EnterRenderLoop();
        }

        protected void CreateRoot()
        {
            mRoot = new Root();
        }

        protected void DefineResources()
        {
            ConfigFile cf = new ConfigFile();
            cf.Load("resources.cfg", "\t:=", true);
            var section = cf.GetSectionIterator();
            while (section.MoveNext())
            {
                foreach (var line in section.Current)
                {
                    ResourceGroupManager.Singleton.AddResourceLocation(line.Value, line.Key, section.CurrentKey);
                }
            }
        }

        protected void CreateRenderSystem()
        {
            if (!mRoot.ShowConfigDialog())
                throw new OperationCanceledException();
        }

        protected void CreateRenderWindow()
        {
            mRenderWindow = mRoot.Initialise(true, "Main_Ogre_Window");
        }

        protected void InitializeResources()
        {
            TextureManager.Singleton.DefaultNumMipmaps = 5;
            ResourceGroupManager.Singleton.InitialiseAllResourceGroups();
        }

        protected virtual void ChooseSceneManager()
        {
            mSceneMgr = mRoot.CreateSceneManager(SceneType.ST_GENERIC);
        }

        protected virtual void CreateCamera()
        {
            mCamera = mSceneMgr.CreateCamera("CameraRTS");

            mCamera.Position = new Vector3(0, 2000, 1000);
            mCamera.LookAt(Vector3.ZERO);

            mCameraRTS = new CameraRTS(mCamera);
        }

        protected virtual void CreateViewports()
        {
            // Create one viewport, entire window
            var vp = mRenderWindow.AddViewport(mCamera);

            // Alter the camera aspect ratio to match the viewport
            mCamera.AspectRatio = (vp.ActualWidth / vp.ActualHeight);
        }


        protected virtual void CreateScene()
        {
            Entity ogreHead = mSceneMgr.CreateEntity("Head", "ogrehead.mesh");
            SceneNode headNode = mSceneMgr.RootSceneNode.CreateChildSceneNode();
            headNode.AttachObject(ogreHead);

            mSceneMgr.AmbientLight = new ColourValue(0.5f, 0.5f, 0.5f);

            Light l = mSceneMgr.CreateLight("MainLight");
            l.Position = new Vector3(20, 80, 50);
        }

        protected void CreateFrameListeners()
        {
            mRoot.FrameRenderingQueued += new FrameListener.FrameRenderingQueuedHandler(OnFrameRenderingQueued);
        }

        protected bool OnFrameRenderingQueued(FrameEvent evt)
        {
            if (mRenderWindow.IsClosed)
            {
                return false;
            }

            if (mKeyboard.IsKeyDown(MOIS.KeyCode.KC_ESCAPE))
            {
                return false;
            }

            ProcessInput();

            UpdateScene(evt);

            mCameraRTS.UpdatePosition(evt.timeSinceLastFrame);

            return true;
        }

        protected void EnterRenderLoop()
        {
            mRoot.StartRendering();
        }

        protected virtual void UpdateScene(FrameEvent evt)
        {
        }
    }
}
