using Mogre;
using System;

namespace sma_ogre
{
    class OgreApp
    {
        protected static Root              mRoot;
        protected static RenderWindow      mRenderWindow;
        protected static MOIS.InputManager mInputMgr;
        protected static MOIS.Keyboard     mKeyboard;
        protected static MOIS.Mouse        mMouse;
        protected static CameraRTS         mainCamera;

        protected static void CreateRoot()
        {
            mRoot = new Root();
        }

        protected static void DefineResources()
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

        protected static void CreateRenderSystem()
        {
            if (!mRoot.ShowConfigDialog())
                throw new OperationCanceledException();
        }

        protected static void CreateRenderWindow()
        {
            mRenderWindow = mRoot.Initialise(true, "Main_Ogre_Window");
        }

        protected static void InitializeResources()
        {
            TextureManager.Singleton.DefaultNumMipmaps = 5;
            ResourceGroupManager.Singleton.InitialiseAllResourceGroups();
        }

        protected static void InitializeInput()
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

        protected static void CreateScene()
        {
            SceneManager sceneMgr = mRoot.CreateSceneManager(SceneType.ST_GENERIC);
            mainCamera = new CameraRTS(sceneMgr);
            mainCamera.display(mRenderWindow);

            Entity ogreHead = sceneMgr.CreateEntity("Head", "ogrehead.mesh");
            SceneNode headNode = sceneMgr.RootSceneNode.CreateChildSceneNode();
            headNode.AttachObject(ogreHead);

            sceneMgr.AmbientLight = new ColourValue(0.5f, 0.5f, 0.5f);

            Light l = sceneMgr.CreateLight("MainLight");
            l.Position = new Vector3(20, 80, 50);
        }

        protected static void CreateFrameListeners()
        {
            mRoot.FrameRenderingQueued += new FrameListener.FrameRenderingQueuedHandler(OnFrameRenderingQueued);
        }

        static bool OnFrameRenderingQueued(FrameEvent evt)
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
                mainCamera.translatePosition(0, 1, 0);
 
            // Move camera backward.
            if(mKeyboard.IsKeyDown(MOIS.KeyCode.KC_DOWN))
                mainCamera.translatePosition(0, -1, 0);
 
            // Move camera left.
            if(mKeyboard.IsKeyDown(MOIS.KeyCode.KC_LEFT))
                mainCamera.translatePosition(-1, 0, 0);
 
            // Move camera right.
            if (mKeyboard.IsKeyDown(MOIS.KeyCode.KC_RIGHT))
                mainCamera.translatePosition(1, 0, 0);

            mainCamera.updatePosition(evt.timeSinceLastFrame);

            if (mMouse.MouseState.ButtonDown(MOIS.MouseButtonID.MB_Right))
            {
                mainCamera.updateRotation(mMouse.MouseState.X.rel, mMouse.MouseState.Y.rel, evt.timeSinceLastFrame);
            }    

            return true;
        }

        protected static void EnterRenderLoop()
        {
            mRoot.StartRendering();
        }
    }
}
