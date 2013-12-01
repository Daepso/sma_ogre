using sma_ogre.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sma_ogre.behavior
{
    class WreckerBehavior : Behavior
    {
        static float pickUpDistance = 20;
        static float dropDistance = 20;

        private List<Item> listItem;
        private Item carriedItem;

        static float minWreckerTime = 2;
        static float maxWreckerTime = 10;
        private Timer wreckerTimer;

        public override void Init()
        {
            base.Init();
            this.ChooseTargetPosition();
        }

        protected bool WreckeAction(float elapsedTime)
        {
            if (carriedItem == null)
            {
                foreach (Item i in listItem)
                {
                    if (i.distance(mAgentNode.Position.x, mAgentNode.Position.z) < pickUpDistance)
                    {
                        carriedItem = i;
                        carriedItem.pickUp();
                        listItem.Remove(carriedItem);
                        return true;
                    }
                }
            }
            else
            {
                foreach (Item i in listItem)
                {
                    if (i.distance(mAgentNode.Position.x, mAgentNode.Position.z) < dropDistance)
                    {
                        carriedItem.drop(mAgentNode.Position.x, mAgentNode.Position.z);
                        listItem.Add(carriedItem);
                        carriedItem = null;
                        return true;
                    }
                }
            }
            return false;
        }

        public override void Update(float elapsedTime)
        {
            MoveToTargetPosition(elapsedTime);

            if (wreckerTimer.isFinished())
            {
                if (WreckeAction(elapsedTime))
                {
                    wreckerTimer.init();
                }
            }
            else
            {
                wreckerTimer.updateTimer(elapsedTime);
            }
        }

        public WreckerBehavior(List<Item> listItem)
        {
            this.listItem = listItem;
            carriedItem = null;
            wreckerTimer = new Timer(minWreckerTime, maxWreckerTime);
            wreckerTimer.init();
        }
    }
}
