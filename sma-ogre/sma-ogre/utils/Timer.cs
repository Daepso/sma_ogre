using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public void init()
        {
            time = WorldConfig.Singleton.RandFloat(minDelay, maxDelay);
        }

        public void updateTimer(float elapsedTime)
        {
            time -= elapsedTime;
        }

        public bool isFinished()
        {
            return time <= 0;
        }
    }
}
