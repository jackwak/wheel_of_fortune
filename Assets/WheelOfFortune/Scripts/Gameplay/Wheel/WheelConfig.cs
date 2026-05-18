using UnityEngine;
using WheelOfFortune.Enums;

namespace WheelOfFortune.Gameplay.Wheel
{
    [CreateAssetMenu(fileName = "WheelConfig", menuName = "WheelOfFortune/Wheel/WheelConfig")]
    public class WheelConfig : ScriptableObject
    {
        [SerializeField] private int _sliceCount = 8;
        [SerializeField] private float _cellRadius = 100f;
        [SerializeField] private Sprite _bombSprite;
        [SerializeField] private BombCountByRank[] _bombCountByRank;

        public int SliceCount => _sliceCount;
        public float CellRadius => _cellRadius;
        public Sprite BombSprite => _bombSprite;
        public BombCountByRank[] BombCountByRank => _bombCountByRank;
    }

    [System.Serializable]
    public struct BombCountByRank
    {
        public Rank Rank;
        public int BombCount;
    }
}