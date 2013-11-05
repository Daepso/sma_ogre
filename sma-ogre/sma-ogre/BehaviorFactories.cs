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
        public BuilderBehaviorFactory() {}

        public override Behavior MakeBehavior()
        {
            return new BuilderBehavior();
        }
    }
}
