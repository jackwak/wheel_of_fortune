using WheelOfFortune.Enums;

namespace WheelOfFortune.Gameplay.Reward
{
    public interface IRewardDataProvider
    {
        public RewardData[] GetRewardDataByRank(Rank rank);
        public RewardData[] GetRandomRewardDataByRank(Rank rank, int count);
    }
}
