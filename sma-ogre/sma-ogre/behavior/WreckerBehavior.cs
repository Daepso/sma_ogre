using sma_ogre.utils;
using System.Collections.Generic;

namespace sma_ogre.behavior
{
    class WreckerBehavior : CarrierBehavior
    {
        static private float pickUpDistance = 20;
        static private float dropDistance   = 20;

        static private float minWreckerTime = 2;
        static private float maxWreckerTime = 10;
        private Timer wreckerTimer;

        public override void Init()
        {
            base.Init();
            this.ChooseTargetPosition();
        }

        public override void ChooseTargetPosition()
        {
            base.ChooseTargetPosition();
            baseSpeed = WorldConfig.Singleton.RandFloat(WorldConfig.Singleton.WreckerSpeedRange[0],
                                                        WorldConfig.Singleton.WreckerSpeedRange[1]);
        }

        protected bool WreckerAction(float elapsedTime)
        {
            if (carriedItem == null)
            {
                foreach (Item i in listItem)
                {
                    if (i.Distance(mAgentNode.Position.x, mAgentNode.Position.z) < pickUpDistance)
                    {
                        pickUpAction(i);
                        return true;
                    }
                }
            }
            else
            {
                bool isEmptySpace = true;
                foreach (Item i in listItem)
                {
                    if (i.Distance(mAgentNode.Position.x, mAgentNode.Position.z) < dropDistance)
                    {
                        isEmptySpace = false;

                    }
                }

                if (isEmptySpace)
                {
                    dropAction(mAgentNode.Position.x, mAgentNode.Position.z);
                    return true;
                }
            }
            return false;
        }

        public override void Update(float elapsedTime, AgentAnimation animation = null)
        {
            speed = baseSpeed * WorldConfig.Singleton.SpeedFactor;
            MoveToTargetPosition(elapsedTime);

            if (wreckerTimer.isFinished())
            {
                if (WreckerAction(elapsedTime))
                {
                    wreckerTimer.init();
                }
            }
            else
            {
                wreckerTimer.updateTimer(elapsedTime);
            }

            if (animation != null)
            {
                animation.UpdatePosture(elapsedTime, speed);
            }
        }

        public WreckerBehavior(List<Item> listItem)
            :base(listItem)
        {
            wreckerTimer = new Timer(minWreckerTime, maxWreckerTime);
            wreckerTimer.init();
        }
    }
}
