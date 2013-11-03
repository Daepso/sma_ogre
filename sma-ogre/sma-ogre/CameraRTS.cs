using Mogre;

namespace sma_ogre
{
    class CameraRTS
    {
        private Camera  camera;
        private Vector3 nextTranslation;
        private float   translateSpeed = 100;
        private float   rotateSpeed    = 0.15f;

        public CameraRTS(SceneManager sceneMgr)
        {
            camera = sceneMgr.CreateCamera("CameraRTS");
            camera.Position = new Vector3(0, 0, 500);
            camera.SetDirection(0, 0, -1);

            nextTranslation = new Vector3(0, 0, 0);
        }

        public void display(RenderWindow mRenderWindow)
        {
            // Create one viewport, entire window
            var vp = mRenderWindow.AddViewport(camera);

            // Alter the camera aspect ratio to match the viewport
            camera.AspectRatio = (vp.ActualWidth / vp.ActualHeight);
        }

        public void translatePosition(int x, int y, int z)
        {
            nextTranslation.x += x;
            nextTranslation.y += y;
            nextTranslation.z += z;
        }

        public void updatePosition(float elapsedTime)
        {
            nextTranslation.Normalise();
            camera.Move(nextTranslation * translateSpeed * elapsedTime);
            nextTranslation.x = 0;
            nextTranslation.y = 0;
            nextTranslation.z = 0;
        }

        public void updateRotation(int x, int y, float elapsedTime)
        {
            camera.Yaw(new Degree(-x * rotateSpeed));
            camera.Pitch(new Degree(-y * rotateSpeed));
        }
    }
}
