using sma_ogre.item;
using sma_ogre.utils;
using System.Collections.Generic;

namespace sma_ogre.behavior
{
    class WreckerBehavior : CarrierBehavior
    {
        protected enum state { SEARCH_OBJECT, DROP_OBJECT, GO_AWAY };

        static private float minWreckerTime = 2;
        static private float maxWreckerTime = 10;

        protected state currentState;

        protected Timer timer;


        public override void ChooseRandomTargetPosition()
        {
            base.ChooseRandomTargetPosition();
            baseSpeed = WorldConfig.Singleton.RandFloat(WorldConfig.Singleton.WreckerSpeedRange[0],
                                                        WorldConfig.Singleton.WreckerSpeedRange[1]);
        }

        protected bool DropObject()
        {
            List<Item> listItem = itemManager.GetItemInSight(mAgentNode.Position.x,
                                                        mAgentNode.Position.z,
                                                        WorldConfig.Singleton.BuilderSightRange);

            if (listItem.Count == 0)
            {
                  DropAction(mAgentNode.Position.x, mAgentNode.Position.z);
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
                        this.currentState = state.GO_AWAY;
                        timer.init();
                    }
                    break;

                case state.DROP_OBJECT:
                    res = DropObject();
                    if (res == true)
                    {
                        this.currentState = state.GO_AWAY;
                        timer.init();
                    }
                    break;

                case state.GO_AWAY:
                    timer.updateTimer(elapsedTime);
                    if (timer.isFinished())
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


        public WreckerBehavior(ItemManager itemManager)
            : base(itemManager)
        {
           timer = new Timer(minWreckerTime, maxWreckerTime);
           timer.init();
        }
    }
}
