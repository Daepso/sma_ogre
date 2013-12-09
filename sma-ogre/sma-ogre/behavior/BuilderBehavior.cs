using sma_ogre.utils;
using sma_ogre.item;
using System.Collections.Generic;

namespace sma_ogre.behavior
{

    class BuilderBehavior : CarrierBehavior
    {
        protected enum state { SEARCH_OBJECT, SEARCH_POSITION, DROP_OBJECT, GO_AWAY };

        static private float actionDistance = 20;
        static private float minSearchTimer = 2;
        static private float maxSearchTimer = 5;

        protected state currentState;

        /* SEARCH POSITION */
        protected Timer searchTimer;
        protected int ObjectInSightMaxNumber = 0;
        protected float betterPositionX;
        protected float betterPositionZ;

        /* DROP OBJECT*/
        protected bool readyToDrop;

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

        protected bool searchObject()
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
                        pickUpAction(i);
                        return true;
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

        protected bool searchPosition()
        {
            List<Item> listItem = itemManager.GetItemInSight(mAgentNode.Position.x,
                                                             mAgentNode.Position.z,
                                                             WorldConfig.Singleton.BuilderSightRange);
            if (listItem.Count > ObjectInSightMaxNumber)
            {
                ObjectInSightMaxNumber = listItem.Count;
                betterPositionX = mAgentNode.Position.x;
                betterPositionZ = mAgentNode.Position.z;
            }


            return searchTimer.isFinished();
        }

        protected bool dropObject()
        {
            if (readyToDrop == false)
            {
                float x = (targetPosition.x - mAgentNode.Position.x);
                float z = (targetPosition.z - mAgentNode.Position.z);
                float dis = (float)System.Math.Sqrt(x * x + z * z);
                if (dis < WorldConfig.Singleton.BuilderSightRange)
                {
                    readyToDrop = true;
                }
            }
            else
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
                            dropAction(mAgentNode.Position.x, mAgentNode.Position.z);
                            return true;
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
            }
            return false;
        }

        public override void Update(float elapsedTime, AgentAnimation animation = null)
        {
            speed = baseSpeed * WorldConfig.Singleton.SpeedFactor;
            MoveToTargetPosition(elapsedTime);
            bool res;

            if (state.SEARCH_OBJECT == this.currentState)
            {
                res = searchObject();
                if (res == true)
                {
                    this.currentState = state.SEARCH_POSITION;
                    searchTimer.init();
                }
            }
            else if (state.SEARCH_POSITION == this.currentState)
            {
                searchTimer.updateTimer(elapsedTime);
                res = searchPosition();
                if (res == true)
                {
                    this.currentState = state.DROP_OBJECT;
                    targetPosition.x = betterPositionX;
                    targetPosition.z = betterPositionZ;
                    readyToDrop = false;
                }
            }
            else if (state.DROP_OBJECT == this.currentState)
            {
                res = dropObject();
                if (res == true)
                {
                    this.currentState = state.GO_AWAY;
                    searchTimer.init();
                }
            }
            else if (state.GO_AWAY == this.currentState)
            {
                searchTimer.updateTimer(elapsedTime);
                if (searchTimer.isFinished())
                {
                    this.currentState = state.SEARCH_OBJECT;
                }
            }

            if (animation != null)
            {
                animation.UpdatePosture(elapsedTime, speed);
            }
        }

        public BuilderBehavior(ItemManager itemManager)
            : base(itemManager)
        {
            searchTimer = new Timer(minSearchTimer, maxSearchTimer);
            searchTimer.init();
            currentState = state.SEARCH_OBJECT;

            ObjectInSightMaxNumber = 0;
        }
    }
}
