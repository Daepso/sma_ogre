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
        List<Item> listItem;

        public BuilderBehaviorFactory(List<Item> listItem)
        {
            this.listItem = listItem;
        }

        public override Behavior MakeBehavior()
        {
            return new BuilderBehavior(listItem);
        }
    }

    class WreckerBehaviorFactory : BehaviorFactory
    {
        List<Item> listItem;

        public WreckerBehaviorFactory(List<Item> listItem)
        {
            this.listItem = listItem;
        }

        public override Behavior MakeBehavior()
        {
            return new WreckerBehavior(listItem);
        }
    }
}
