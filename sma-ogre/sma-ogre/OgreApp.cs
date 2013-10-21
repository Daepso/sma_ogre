using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Mogre;

namespace sma_ogre
{
    class OgreApp
    {
        protected static Root mRoot;
        protected static RenderWindow mRenderWindow;
        protected static MOIS.InputManager mInputMgr;
        protected static MOIS.Keyboard mKeyboard;
        protected static MOIS.Mouse mMouse;
        protected static float mTimer = 5;

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
            Camera camera = sceneMgr.CreateCamera("Camera");
            camera.Position = new Vector3(0, 0, 150);
            camera.LookAt(Vector3.ZERO);
            mRenderWindow.AddViewport(camera);

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

           if (mKeyboard.IsKeyDown(MOIS.KeyCode.KC_Q))
           {
               return false;
           }
            
           
           return true;
        }

        protected static void EnterRenderLoop()
        {
            mRoot.StartRendering();
        }
    }
}
