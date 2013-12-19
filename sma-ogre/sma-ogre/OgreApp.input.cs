using MOIS;

namespace sma_ogre
{
    partial class OgreApp
    {
        protected Keyboard mKeyboard;
        protected Mouse    mMouse;
        protected bool     mShiftKeyPressed;

        protected void InitializeInput()
        {
            int windowHandle;
            mRenderWindow.GetCustomAttribute("WINDOW", out windowHandle);
            try
            {
                InputManager mInputMgr = MOIS.InputManager.CreateInputSystem((uint)windowHandle);
                mKeyboard = (MOIS.Keyboard)mInputMgr.CreateInputObject(MOIS.Type.OISKeyboard, true);
                mMouse    = (MOIS.Mouse)   mInputMgr.CreateInputObject(MOIS.Type.OISMouse,    true);
            }
            catch (System.Runtime.InteropServices.SEHException e)
            {
                System.Console.Out.WriteLine(e.ToString());
            }

            mKeyboard.KeyPressed  += new KeyListener.KeyPressedHandler(     OnKeyPressed   );
            mKeyboard.KeyReleased += new KeyListener.KeyReleasedHandler(    OnKeyReleased  );
            mMouse.MouseMoved     += new MouseListener.MouseMovedHandler(   OnMouseMoved   );
            mMouse.MousePressed   += new MouseListener.MousePressedHandler( OnMousePressed );
            mMouse.MouseReleased  += new MouseListener.MouseReleasedHandler(OnMouseReleased);
        }

        protected void ProcessInput()
        {
            mKeyboard.Capture();
            mMouse.Capture();
        }

        protected bool OnKeyPressed(KeyEvent evt)
        {
            switch (evt.key)
            {
                // Camera movement
                case MOIS.KeyCode.KC_Z:
                case MOIS.KeyCode.KC_UP:
                    mCameraMan.GoingForward = true;
                    break;

                case MOIS.KeyCode.KC_S:
                case MOIS.KeyCode.KC_DOWN:
                    mCameraMan.GoingBack = true;
                    break;

                case MOIS.KeyCode.KC_A:
                    mCameraMan.GoingUp = true;
                    break;

                case MOIS.KeyCode.KC_E:
                    mCameraMan.GoingDown = true;
                    break;

                case MOIS.KeyCode.KC_LEFT:
                case MOIS.KeyCode.KC_Q:
                    mCameraMan.GoingLeft = true;
                    break;

                case MOIS.KeyCode.KC_RIGHT:
                case MOIS.KeyCode.KC_D:
                    mCameraMan.GoingRight = true;
                    break;

                // World properties
                case MOIS.KeyCode.KC_L:
                    mSceneMgr.AmbientLight = WorldConfig.Singleton.SwitchedLight();
                    break;

                case MOIS.KeyCode.KC_P:
                    if (mShiftKeyPressed)
                    {
                        WorldConfig.Singleton.SwitchPause();
                    }
                    else
                    {
                        WorldConfig.Singleton.SpeedFactorIncrease();
                    }
                    break;

                case MOIS.KeyCode.KC_M:
                    WorldConfig.Singleton.SpeedFactorDecrease();
                    break;

                // Overlays
                case MOIS.KeyCode.KC_H:
                    OverlayUtils.Singleton.ToggleHelp();
                    break;

                case MOIS.KeyCode.KC_I:
                    OverlayUtils.Singleton.ToggleInformation();
                    break;

                // Key modifiers
                case MOIS.KeyCode.KC_LSHIFT:
                case MOIS.KeyCode.KC_RSHIFT:
                    mCameraMan.FastMove = true;
                    mShiftKeyPressed = true;
                    break;
            }

            return true;
        }

        protected bool OnKeyReleased(KeyEvent evt)
        {
            switch (evt.key)
            {
                case MOIS.KeyCode.KC_Z:
                case MOIS.KeyCode.KC_UP:
                    mCameraMan.GoingForward = false;
                    break;

                case MOIS.KeyCode.KC_S:
                case MOIS.KeyCode.KC_DOWN:
                    mCameraMan.GoingBack = false;
                    break;

                case MOIS.KeyCode.KC_A:
                    mCameraMan.GoingUp = false;
                    break;

                case MOIS.KeyCode.KC_E:
                    mCameraMan.GoingDown = false;
                    break;

                case MOIS.KeyCode.KC_LEFT:
                case MOIS.KeyCode.KC_Q:
                    mCameraMan.GoingLeft = false;
                    break;

                case MOIS.KeyCode.KC_RIGHT:
                case MOIS.KeyCode.KC_D:
                    mCameraMan.GoingRight = false;
                    break;

                case MOIS.KeyCode.KC_LSHIFT:
                case MOIS.KeyCode.KC_RSHIFT:
                    mCameraMan.FastMove = false;
                    mShiftKeyPressed = false;
                    break;
            }

            return true;
        }

        protected bool OnMouseMoved(MouseEvent evt)
        {
            //if (mMouse.MouseState.ButtonDown(MOIS.MouseButtonID.MB_Right))
            //{
            mCameraMan.MouseMovement(evt.state.X.rel, evt.state.Y.rel);
            //}
            return true;
        }

        protected bool OnMousePressed(MouseEvent evt, MouseButtonID id)
        {
            return true;
        }

        protected bool OnMouseReleased(MouseEvent evt, MouseButtonID id)
        {
            return true;
        }
    }
}
