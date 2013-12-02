using Mogre;

namespace sma_ogre
{
    class CameraMan
    {
        private Camera mCamera;
        private bool   mGoingForward;
        private bool   mGoingBack;
        private bool   mGoingRight;
        private bool   mGoingLeft;
        private bool   mGoingUp;
        private bool   mGoingDown;
        private bool   mFastMove;

        private float  translateSpeed = 500;
        private float  rotateSpeed    = 0.15f;

        public CameraMan(Camera camera)
        {
            mCamera = camera;
        }

        public bool GoingForward
        {
            set { mGoingForward = value; }
            get { return mGoingForward; }
        }

        public bool GoingBack
        {
            set { mGoingBack = value; }
            get { return mGoingBack; }
        }

        public bool GoingLeft
        {
            set { mGoingLeft = value; }
            get { return mGoingLeft; }
        }

        public bool GoingRight
        {
            set { mGoingRight = value; }
            get { return mGoingRight; }
        }

        public bool GoingUp
        {
            set { mGoingUp = value; }
            get { return mGoingUp; }
        }

        public bool GoingDown
        {
            set { mGoingDown = value; }
            get { return mGoingDown; }
        }

        public bool FastMove
        {
            set { mFastMove = value; }
            get { return mFastMove; }
        }

        public void UpdatePosition(float elapsedTime)
        {
            Vector3 move = Vector3.ZERO;
            if (mGoingForward) move += mCamera.Direction;
            if (mGoingBack)    move -= mCamera.Direction;
            if (mGoingRight)   move += mCamera.Right;
            if (mGoingLeft)    move -= mCamera.Right;
            if (mGoingUp)      move += mCamera.Up;
            if (mGoingDown)    move -= mCamera.Up;

            move.Normalise();
            if (FastMove)
                move *= 3;

            mCamera.Move(move * translateSpeed * elapsedTime);
        }

        public void MouseMovement(int x, int y)
        {
            mCamera.Yaw(  new Degree(-x * rotateSpeed));
            mCamera.Pitch(new Degree(-y * rotateSpeed));
        }
    }
}
