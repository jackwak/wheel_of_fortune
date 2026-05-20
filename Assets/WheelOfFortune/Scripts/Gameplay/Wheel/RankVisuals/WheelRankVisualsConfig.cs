using System;
using UnityEngine;
using WheelOfFortune.Enums;

namespace WheelOfFortune.Gameplay.Wheel.States
{
    [CreateAssetMenu(fileName = "WheelRankStatesConfig", menuName = "WheelOfFortune/Wheel/RankStatesConfig")]
    public class WheelRankVisualsConfig : ScriptableObject
    {
        [SerializeField] private RankVisualData[] _entries;

        public bool TryGet(Rank rank, out RankVisualData data)
        {
            foreach (RankVisualData entry in _entries)
            {
                if (entry.Rank == rank)
                {
                    data = entry;
                    return true;
                }
            }

            data = default;
            return false;
        }
    }

    [Serializable]
    public struct RankVisualData
    {
        public Rank Rank;
        public Sprite WheelSprite;
        public Sprite IndicatorSprite;
        public string Title;
    }
}
