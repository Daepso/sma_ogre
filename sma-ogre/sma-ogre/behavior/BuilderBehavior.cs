using sma_ogre.utils;
using sma_ogre.item;
using System.Collections.Generic;

namespace sma_ogre.behavior
{
    class BuilderBehavior : CarrierBehavior
    {
        static private float actionDistance = 20;

        static private float minCarriedTimer = 2;
        static private float maxCarriedTimer = 10;
        private Timer carriedTimer;

        public override void Init()
        {
            base.Init();
            this.ChooseRandomTargetPosition();
        }

        public override void ChooseRandomTargetPosition()
        {
            base.ChooseRandomTargetPosition();
            baseSpeed = WorldConfig.Singleton.RandFloat(WorldConfig.Singleton.BuilderSpeedRange[0],
                                                        WorldConfig.Singleton.BuilderSpeedRange[1]);
        }

        protected bool BuildAction(float elapsedTime)
        {
            List<Item> listItem = itemManager.GetItemInSight(mAgentNode.Position.x,
                                                             mAgentNode.Position.z,
                                                             WorldConfig.Singleton.BuilderSightRange);

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
                        else
                        {
                            dropAction(mAgentNode.Position.x, mAgentNode.Position.z);
                            return true;
                        }
                    }
                    else
                    {
                        minDis = dis;
                        closestItem = i;
                    }
                }
            }

            if (closestItem != null)
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

            if (carriedTimer.isFinished())
            {
                if (BuildAction(elapsedTime))
                {
                    carriedTimer.init();
                }
            }
            else
            {
                carriedTimer.updateTimer(elapsedTime);
            }

            if (animation != null)
            {
                animation.UpdatePosture(elapsedTime, speed);
            }
        }

        public BuilderBehavior(ItemManager itemManager)
            : base(itemManager)
        {
            carriedTimer = new Timer(minCarriedTimer, maxCarriedTimer);
            carriedTimer.init();
        }
    }
}
