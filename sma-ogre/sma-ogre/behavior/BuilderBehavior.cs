using sma_ogre.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sma_ogre.behavior
{
    class BuilderBehavior : Behavior
    {
        static float pickUpDistance = 20;
        static float dropDistance = 20;

        private List<Item> listItem;
        private Item carriedItem;

        private float minCarriedTimer = 2;
        private float maxCarriedTimer = 10;
        private Timer carriedTimer;

        public override void Init()
        {
            base.Init();
            this.ChooseTargetPosition();
        }

        public override void ChooseTargetPosition()
        {
            base.ChooseTargetPosition();
            baseSpeed = WorldConfig.Singleton.RandFloat(WorldConfig.Singleton.BuilderSpeedRange[0],
                                                        WorldConfig.Singleton.BuilderSpeedRange[1]);
        }

        protected bool BuildAction(float elapsedTime)
        {
            if (carriedItem == null)
            {
                foreach (Item i in listItem)
                {
                    if (i.Distance(mAgentNode.Position.x, mAgentNode.Position.z) < pickUpDistance)
                    {
                        carriedItem = i;
                        carriedItem.PickUp();
                        listItem.Remove(carriedItem);
                        return true;
                    }
                }
            }
            else
            {
                foreach (Item i in listItem)
                {
                    if (i.Distance(mAgentNode.Position.x, mAgentNode.Position.z) < dropDistance)
                    {
                        carriedItem.Drop(mAgentNode.Position.x, mAgentNode.Position.z);
                        listItem.Add(carriedItem);
                        carriedItem = null;
                        return true;
                    }
                }
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

        public BuilderBehavior(List<Item> listItem)
        {
            this.listItem = listItem;
            carriedItem = null;
            carriedTimer = new Timer(minCarriedTimer, maxCarriedTimer);
            carriedTimer.init();
        }
    }
}
