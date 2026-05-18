using WheelOfFortune.Enums;

public interface IRewardDataProvider
{
    public RewardData[] GetRewardDataByRank(Rank rank);
    public RewardData[] GetRandomRewardDataByRank(Rank rank, int count);
}

