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

            foreach (var rankData in _rankConfig.RankData)
            {
                if (levelNumber % rankData.RankInterval == 0)
                    return rankData.Rank;
            }

            return Rank.Bronze;
        }
    }
}