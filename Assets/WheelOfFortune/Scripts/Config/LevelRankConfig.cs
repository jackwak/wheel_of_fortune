using UnityEngine;
using WheelOfFortune.Enums;
namespace WheelOfFortune.Config
{
    [CreateAssetMenu(fileName = "LevelRankConfig", menuName = "WheelOfFortune/Config/LevelRankConfig")]
    public class LevelRankConfig : ScriptableObject
    {
        [SerializeField] private LevelRankData[] _rankData;

        public LevelRankData[] RankDatas => _rankData;
    }
    [System.Serializable]
    public struct LevelRankData
    {
        public int RankInterval;
        public Rank Rank;
    }
}