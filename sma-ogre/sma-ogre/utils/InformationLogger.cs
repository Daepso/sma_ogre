namespace sma_ogre.utils
{
    class InformationLogger
    {
        static private InformationLogger singleton;

        private int robotsNumber;
        private int ogresNumber;

        private InformationLogger()
        {
            robotsNumber = 0;
            ogresNumber  = 0;
        }

        public static InformationLogger Singleton
        {
            get
            {
                if (singleton == null)
                {
                    singleton = new InformationLogger();
                }
                return singleton;
            }
        }

        public int RobotsNumber
        {
            get { return robotsNumber; }
            set { robotsNumber = value;  }
        }

        public int OgresNumber
        {
            get { return ogresNumber; }
            set { ogresNumber = value;  }
        }
    }
}
