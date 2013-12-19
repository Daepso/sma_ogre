namespace sma_ogre.utils
{
    class Timer
    {
        private float minDelay;
        private float maxDelay;
        private float time;

        public Timer(float min, float max)
        {
            minDelay = min;
            maxDelay = max;
            time = 0;
        }

        public void Init()
        {
            time = WorldConfig.Singleton.RandFloat(minDelay, maxDelay);
        }

        public void UpdateTimer(float elapsedTime)
        {
            time -= elapsedTime;
        }

        public bool IsFinished()
        {
            return time <= 0;
        }
    }
}
