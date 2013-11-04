using Mogre;
using System;

namespace sma_ogre
{
    class OgreApp
    {
        protected Root              mRoot;
        protected SceneManager      mSceneMgr;
        protected RenderWindow      mRenderWindow;
        protected MOIS.InputManager mInputMgr;
        protected MOIS.Keyboard     mKeyboard;
        protected MOIS.Mouse        mMouse;
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

        protected void InitializeInput()
        {
            int windowHandle;
            mRenderWindow.GetCustomAttribute("WINDOW", out windowHandle);
            try
            {
                mInputMgr = MOIS.InputManager.CreateInputSystem((uint)windowHandle);
            }
            catch (System.Runtime.InteropServices.SEHException e)
            {
                Console.Out.WriteLine(e.ToString());
            }

            mKeyboard = (MOIS.Keyboard)mInputMgr.CreateInputObject(MOIS.Type.OISKeyboard, false);
            mMouse = (MOIS.Mouse)mInputMgr.CreateInputObject(MOIS.Type.OISMouse, false);
        }

        protected virtual void ChooseSceneManager()
        {
            mSceneMgr = mRoot.CreateSceneManager(SceneType.ST_GENERIC);
        }

        protected virtual void CreateCamera()
        {
            mCamera = mSceneMgr.CreateCamera("CameraRTS");

            mCamera.Position = new Vector3(0, 0, 500);
            mCamera.SetDirection(0, 0, -1);

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
            mKeyboard.Capture();
            mMouse.Capture();

            if (mRenderWindow.IsClosed)
            {
                return false;
            }

            if (mKeyboard.IsKeyDown(MOIS.KeyCode.KC_ESCAPE))
            {
                return false;
            }

            // Move camera forward.
            if (mKeyboard.IsKeyDown(MOIS.KeyCode.KC_UP))
                mCameraRTS.TranslatePosition(0, 1, 0);
 
            // Move camera backward.
            if(mKeyboard.IsKeyDown(MOIS.KeyCode.KC_DOWN))
                mCameraRTS.TranslatePosition(0, -1, 0);
 
            // Move camera left.
            if(mKeyboard.IsKeyDown(MOIS.KeyCode.KC_LEFT))
                mCameraRTS.TranslatePosition(-1, 0, 0);
 
            // Move camera right.
            if (mKeyboard.IsKeyDown(MOIS.KeyCode.KC_RIGHT))
                mCameraRTS.TranslatePosition(1, 0, 0);

            mCameraRTS.UpdatePosition(evt.timeSinceLastFrame);

            if (mMouse.MouseState.ButtonDown(MOIS.MouseButtonID.MB_Right))
            {
                mCameraRTS.UpdateRotation(mMouse.MouseState.X.rel, mMouse.MouseState.Y.rel, evt.timeSinceLastFrame);
            }    

            return true;
        }

        protected void EnterRenderLoop()
        {
            mRoot.StartRendering();
        }
    }
}
