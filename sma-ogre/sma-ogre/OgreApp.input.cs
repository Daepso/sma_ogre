using MOIS;

namespace sma_ogre
{
    partial class OgreApp
    {
        protected Keyboard     mKeyboard;
        protected Mouse        mMouse;

        protected void InitializeInput()
        {
            int windowHandle;
            mRenderWindow.GetCustomAttribute("WINDOW", out windowHandle);
            try
            {
                InputManager mInputMgr = MOIS.InputManager.CreateInputSystem((uint)windowHandle);
                mKeyboard = (MOIS.Keyboard)mInputMgr.CreateInputObject(MOIS.Type.OISKeyboard, false);
                mMouse = (MOIS.Mouse)mInputMgr.CreateInputObject(MOIS.Type.OISMouse, false);
            }
            catch (System.Runtime.InteropServices.SEHException e)
            {
                System.Console.Out.WriteLine(e.ToString());
            }
        }

        protected void ProcessInput()
        {
            mKeyboard.Capture();
            mMouse.Capture();

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

            if (mMouse.MouseState.ButtonDown(MOIS.MouseButtonID.MB_Right))
            {
                mCameraRTS.UpdateRotation(mMouse.MouseState.X.rel, mMouse.MouseState.Y.rel);
            }
        }
    }
}
