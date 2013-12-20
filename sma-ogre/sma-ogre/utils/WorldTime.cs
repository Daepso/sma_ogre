namespace sma_ogre.utils
{
    class WorldTime
    {
        static private WorldTime singleton;

        private float time;
        private float speedFactor;
        private bool  pause;

        private WorldTime()
        {
            time        = 0;
            speedFactor = 1f;
            pause       = true;
        }

        public static WorldTime Singleton
        {
            get
            {
                if (singleton == null)
                {
                    singleton = new WorldTime();
                }
                return singleton;
            }
        }

        // Global time handling
        public void UpdateTime(float elapsedTime)
        {
            time += elapsedTime;
        }

        public float GetTime()
        {
            return time;
        }

        public string GetTimeString()
        {
            return time.ToString("0.000");
        }

        // Global speed factor handlers
        public float SpeedFactor
        {
            get { return speedFactor; }
        }

        public void SpeedFactorDecrease()
        {
            if (speedFactor > 0.01)
            {
                speedFactor /= 2;
            }
        }

        public void SpeedFactorIncrease()
        {
            if (speedFactor < 100)
            {
                speedFactor *= 2;
            }
        }

        // Pause functions
        public bool Pause
        {
            get { return pause; }
        }

        public void SwitchPause()
        {
            pause = !pause;
        }
    }
}
