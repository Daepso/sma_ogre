using System.Collections.Generic;
using sma_ogre.item;

namespace sma_ogre.behavior
{
    class BehaviorFactory
    {
        public BehaviorFactory() {}

        public virtual Behavior MakeBehavior()
        {
            return new Behavior();
        }
    }

    class BuilderBehaviorFactory : BehaviorFactory
    {
        ItemManager itemManager;

        public BuilderBehaviorFactory(ItemManager itemManager)
        {
            this.itemManager = itemManager;
        }

        public override Behavior MakeBehavior()
        {
            return new BuilderBehavior(itemManager);
        }
    }

    class WreckerBehaviorFactory : BehaviorFactory
    {
        ItemManager itemManager;

        public WreckerBehaviorFactory(ItemManager itemManager)
        {
            this.itemManager = itemManager;
        }

        public override Behavior MakeBehavior()
        {
            return new WreckerBehavior(itemManager);
        }
    }
}
