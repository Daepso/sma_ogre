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
            List<Item> listItem = itemManager.GetItemInSight(mAgentNode.Position.x,
                                                            mAgentNode.Position.z,
                                                            WorldConfig.Singleton.BuilderSightRange);
            if (carriedItem != null)
            {
                float dis = Utils.distanceXZ(targetPosition, mAgentNode.Position);
                if (dis < actionDistance)
                {
                    dropAction(mAgentNode.Position.x, mAgentNode.Position.z);
                }
            }

            if(isAtTargetPosition)
            {
                ObjectInSightMaxNumber = listItem.Count;
                return true;
            }
  
            return false;
        }

        protected void stateAction(float elapsedTime)
        {
            bool res;
            switch (this.currentState)
            {

                case state.SEARCH_OBJECT:
                    res = searchObject();
                    if (res == true)
                    {
                        this.currentState = state.SEARCH_POSITION;
                        searchTimer.init();
                    }
                    break;

                case state.SEARCH_POSITION:
                    searchTimer.updateTimer(elapsedTime);
                    res = searchPosition();
                    if (res == true)
                    {
                        this.currentState = state.DROP_OBJECT;
                        targetPosition.x = betterPositionX;
                        targetPosition.z = betterPositionZ;
                    }
                    break;

                case state.DROP_OBJECT:
                    res = dropObject();
                    if (res == true)
                    {
                        this.currentState = state.GO_AWAY;
                        searchTimer.init();
                    }
                    break;

                case state.GO_AWAY:
                    searchTimer.updateTimer(elapsedTime);
                    if (searchTimer.isFinished())
                    {
                        this.currentState = state.SEARCH_OBJECT;
                    }
                    break;
            }
        }

        public override void Update(float elapsedTime, AgentAnimation animation = null)
        {
            speed = baseSpeed * WorldConfig.Singleton.SpeedFactor;
            MoveToTargetPosition(elapsedTime);
            stateAction(elapsedTime);
            if (isAtTargetPosition)
            {
                this.ChooseRandomTargetPosition();
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
