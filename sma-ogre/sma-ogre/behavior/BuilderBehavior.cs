using sma_ogre.utils;
using sma_ogre.item;
using System.Collections.Generic;

namespace sma_ogre.behavior
{

    class BuilderBehavior : CarrierBehavior
    {
        protected enum state { SEARCH_OBJECT, DROP_OBJECT, GO_AWAY };

        static private float minSearchTimer = 2;
        static private float maxSearchTimer = 5;

        protected state currentState;

        protected Timer timer;

        public override void ChooseRandomTargetPosition()
        {
            base.ChooseRandomTargetPosition();
            baseSpeed = WorldConfig.Singleton.RandFloat(WorldConfig.Singleton.BuilderSpeedRange[0],
                                                        WorldConfig.Singleton.BuilderSpeedRange[1]);
        }


        protected bool DropObjectAction()
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
                        DropAction(mAgentNode.Position.x, mAgentNode.Position.z);
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
                targetPosition.x = closestItem.GetPositionX();
                targetPosition.z = closestItem.GetPositionZ();
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
                        this.currentState = state.GO_AWAY;
                        timer.Init();
                    }
                    break;

                case state.DROP_OBJECT:
                    res = DropObjectAction();
                    if (res == true)
                    {
                        this.currentState = state.GO_AWAY;
                        timer.Init();
                    }
                    break;

                case state.GO_AWAY:
                    timer.UpdateTimer(elapsedTime);
                    if (timer.IsFinished())
                    {
                        if (carriedItem == null)
                        { 
                            this.currentState = state.SEARCH_OBJECT; 
                        }
                        else
                        {
                            this.currentState = state.DROP_OBJECT; 
                        }
                    }
                    break;
            }
        }

        public override void Update(float elapsedTime, AgentAnimation animation = null)
        {
            speed = baseSpeed * WorldConfig.Singleton.SpeedFactor;
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

        public BuilderBehavior(ItemManager itemManager)
            : base(itemManager)
        {
            timer = new Timer(minSearchTimer, maxSearchTimer);
            timer.Init();
            currentState = state.SEARCH_OBJECT;
        }
    }
}
