using sma_ogre.item;
using sma_ogre.utils;
using System.Collections.Generic;

namespace sma_ogre.behavior
{
    class WreckerBehavior : CarrierBehavior
    {
        static private float actionDistance = 20;

        static private float minWreckerTime = 2;
        static private float maxWreckerTime = 10;
        private Timer wreckerTimer;

        public override void Init()
        {
            base.Init();
            this.ChooseRandomTargetPosition();
        }

        public override void ChooseRandomTargetPosition()
        {
            base.ChooseRandomTargetPosition();
            baseSpeed = WorldConfig.Singleton.RandFloat(WorldConfig.Singleton.WreckerSpeedRange[0],
                                                        WorldConfig.Singleton.WreckerSpeedRange[1]);
        }

        protected bool WreckerAction(float elapsedTime)
        {
            List<Item> listItem = itemManager.GetItemInSight(mAgentNode.Position.x,
                                                             mAgentNode.Position.z,
                                                              WorldConfig.Singleton.WreckerSightRange);
            float minDis = float.MaxValue;
            Item closestItem = null;

            foreach (Item i in listItem)
            {
                float dis = i.Distance(mAgentNode.Position.x, mAgentNode.Position.z);
                if (dis < minDis)
                {

                    if (dis < actionDistance)
                    {
                        if (carriedItem == null)
                        {
                            pickUpAction(i);
                            return true;
                        }
                    }
                    minDis = dis;
                    closestItem = i;
                }
            }

            if (closestItem == null && carriedItem != null)
            {
                dropAction(mAgentNode.Position.x, mAgentNode.Position.z);
                return true;
            }
            else if (closestItem != null && carriedItem == null)
            {
                targetPosition.x = closestItem.getPositionX();
                targetPosition.z = closestItem.getPositionZ();
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

        public WreckerBehavior(ItemManager itemManager)
            : base(itemManager)
        {
            wreckerTimer = new Timer(minWreckerTime, maxWreckerTime);
            wreckerTimer.init();
        }
    }
}
