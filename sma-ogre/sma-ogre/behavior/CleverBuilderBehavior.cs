using sma_ogre.utils;
using sma_ogre.item;
using System.Collections.Generic;

namespace sma_ogre.behavior
{
    class CleverBuilderBehavior : CarrierBehavior
    {
        protected enum state { SEARCH_OBJECT, SEARCH_POSITION, DROP_OBJECT, GO_AWAY };


        static private float minSearchTimer = 2;
        static private float maxSearchTimer = 5;

        protected state currentState;

        /* SEARCH POSITION */
        protected Timer searchTimer;
        protected int ObjectInSightMaxNumber = 0;
        protected float betterPositionX;
        protected float betterPositionZ;


        public override void ChooseRandomTargetPosition()
        {
            base.ChooseRandomTargetPosition();
            baseSpeed = WorldConfig.Singleton.RandFloat(WorldConfig.Singleton.BuilderSpeedRange[0],
                                                        WorldConfig.Singleton.BuilderSpeedRange[1]);
        }

        protected bool SearchPosition()
        {
            List<Item> listItem = itemManager.GetItemInSight(mAgentNode.Position.x,
                                                             mAgentNode.Position.z,
                                                             RealSightRange(WorldConfig.Singleton.BuilderSightRange));
            if (listItem.Count > ObjectInSightMaxNumber)
            {
                ObjectInSightMaxNumber = listItem.Count;
                betterPositionX = mAgentNode.Position.x;
                betterPositionZ = mAgentNode.Position.z;
            }


            return searchTimer.IsFinished();
        }

        protected bool DropObjectAction()
        {       
            if (carriedItem != null)
            {
                float dis = Utils.distanceXZ(targetPosition, mAgentNode.Position);
                if (dis < actionDistance)
                {
                    DropAction(mAgentNode.Position.x, mAgentNode.Position.z);
                }
            }

            if(isAtTargetPosition)
            {
                List<Item> listItem = itemManager.GetItemInSight(mAgentNode.Position.x,
                                                                 mAgentNode.Position.z,
                                                                 RealSightRange(WorldConfig.Singleton.BuilderSightRange));
                ObjectInSightMaxNumber = listItem.Count;
                return true;
            }
  
            return false;
        }

        protected void StateAction(float elapsedTime)
        {
            bool res;
            switch (this.currentState)
            {

                case state.SEARCH_OBJECT:
                    res = SearchObjectAction();
                    if (res == true)
                    {
                        this.currentState = state.SEARCH_POSITION;
                        searchTimer.Init();
                    }
                    break;

                case state.SEARCH_POSITION:
                    searchTimer.UpdateTimer(elapsedTime);
                    res = SearchPosition();
                    if (res == true)
                    {
                        this.currentState = state.DROP_OBJECT;
                        targetPosition.x = betterPositionX;
                        targetPosition.z = betterPositionZ;
                    }
                    break;

                case state.DROP_OBJECT:
                    res = DropObjectAction();
                    if (res == true)
                    {
                        this.currentState = state.GO_AWAY;
                        searchTimer.Init();
                    }
                    break;

                case state.GO_AWAY:
                    searchTimer.UpdateTimer(elapsedTime);
                    if (searchTimer.IsFinished())
                    {
                        this.currentState = state.SEARCH_OBJECT;
                    }
                    break;
            }
        }

        public override void Update(float elapsedTime, AgentAnimation animation = null)
        {
            AdjustSpeed();
            MoveToTargetPosition(elapsedTime);
            StateAction(elapsedTime);
            if (isAtTargetPosition)
            {
                this.ChooseRandomTargetPosition();
            }

            if (animation != null)
            {
                animation.UpdatePosture(elapsedTime, speed);
            }
        }

        public CleverBuilderBehavior(ItemManager itemManager)
            : base(itemManager)
        {
            searchTimer = new Timer(minSearchTimer, maxSearchTimer);
            searchTimer.Init();
            currentState = state.SEARCH_OBJECT;

            ObjectInSightMaxNumber = 0;
        }
    }
}
