using Mogre;

namespace sma_ogre
{
    class AgentAnimation
    {
        private AnimationState mAnimationState = null;

        public AgentAnimation(Entity entity)
        {
            mAnimationState         = entity.GetAnimationState("Walk");
            mAnimationState.Loop    = true;
            mAnimationState.Enabled = true;
        }

        public void UpdatePosture(float elapsedTime, float agentSpeed)
        {
            mAnimationState.AddTime(elapsedTime * agentSpeed / 40);
        }
    }
}
