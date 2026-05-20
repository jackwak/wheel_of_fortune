using WheelOfFortune.Gameplay.Reward;

namespace WheelOfFortune.Gameplay.RewardMoveEffectManager
{
    public struct OnRewardEffectArrivedEvent
    {
        public RewardData RewardData { get; }

        public OnRewardEffectArrivedEvent(RewardData rewardData)
        {
            RewardData = rewardData;
        }
    }
}