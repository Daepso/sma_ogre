using System.Collections.Generic;

namespace sma_ogre
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
}
