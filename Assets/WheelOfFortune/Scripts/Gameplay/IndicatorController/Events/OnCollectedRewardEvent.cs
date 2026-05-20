using WheelOfFortune.Gameplay.Reward;

namespace WheelOfFortune.Gameplay.IndicatorController.Events
{
    public struct OnCollectedRewardEvent
    {
        public RewardData RewardData { get; }

        public OnCollectedRewardEvent(RewardData rewardData)
        {
            RewardData = rewardData;
        }
    }
}