namespace sma_ogre.utils
{
    class WorldTime
    {
        static private WorldTime singleton;
        static private float time;

        private WorldTime()
        {
            time = 0;
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

        public void UpdateTime(float elapsedTime)
        {
            time += elapsedTime;
        }

        public float GetTime()
        {
            return time;
        }
    }
}
