using UnityEngine;
using WheelOfFortune.Enums;

namespace WheelOfFortune.Gameplay.Reward
{
    [CreateAssetMenu(fileName = "RewardCatalogEntry", menuName = "WheelOfFortune/Gameplay/Reward/RewardCatalogEntry")]
    public class RewardCatalogEntry : ScriptableObject
    {
        [SerializeField] private RewardDefinition _definition;
        [SerializeField] private Rank _rank;
        [SerializeField] private Vector2Int _countRange;

        public RewardDefinition Definition => _definition;
        public Rank Rank => _rank;
        public Vector2Int CountRange => _countRange;
    }
}
