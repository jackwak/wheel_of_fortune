using UnityEngine;
using WheelOfFortune.Enums;
using WheelOfFortune.Config;

namespace WheelOfFortune.Utils.RankDeterminer
{
    public class RankDeterminer : MonoBehaviour
    {
        [SerializeField] private LevelRankConfig _rankConfig;

        public Rank DetermineRank(int levelNumber)
        {
            if (levelNumber == 1)
                return Rank.Silver;

            Rank resultRank = Rank.Bronze;
            int highestMatchedInterval = 0;

            foreach (LevelRankData rankData in _rankConfig.RankDatas)
            {
                if (rankData.RankInterval <= 0)
                {
                    Debug.LogWarning($"Invalid rank interval: {rankData.RankInterval}");
                    continue;
                }

                if (levelNumber % rankData.RankInterval != 0)
                    continue;

                if (rankData.RankInterval > highestMatchedInterval)
                {
                    highestMatchedInterval = rankData.RankInterval;
                    resultRank = rankData.Rank;
                }
            }

            return resultRank;
        }
    }
}