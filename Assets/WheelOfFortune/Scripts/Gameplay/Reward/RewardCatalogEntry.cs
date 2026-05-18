using UnityEngine;
using WheelOfFortune.Enums;

[CreateAssetMenu(fileName = "RewardCatalogEntry", menuName = "WheelOfFortune/Rewards/Reward Catalog Entry")]
public class RewardCatalogEntry : ScriptableObject
{
    [SerializeField] private RewardDefinition _definition;
    [SerializeField] private Rank _rank;
    [SerializeField] private Vector2Int _countRange;

    public RewardDefinition Definition => _definition;
    public Rank Rank => _rank;
    public Vector2Int CountRange => _countRange;
}

