using Mogre;

namespace sma_ogre
{
    class CameraRTS
    {
        private Camera  mCamera;
        private Vector3 nextTranslation;
        private float   translateSpeed = 1000;
        private float   rotateSpeed    = 0.15f;

        public CameraRTS(Camera camera)
        {
            mCamera = camera;

            nextTranslation = new Vector3(0, 0, 0);
        }

        public void TranslatePosition(int x, int y, int z)
        {
            nextTranslation.x += x;
            nextTranslation.y += y;
            nextTranslation.z += z;
        }

        public void UpdatePosition(float elapsedTime)
        {
            nextTranslation.Normalise();
            mCamera.Move(nextTranslation * translateSpeed * elapsedTime);
            nextTranslation.x = 0;
            nextTranslation.y = 0;
            nextTranslation.z = 0;
        }

        public void UpdateRotation(int x, int y)
        {
            mCamera.Yaw(new Degree(-x * rotateSpeed));
            mCamera.Pitch(new Degree(-y * rotateSpeed));
        }
    }
}
