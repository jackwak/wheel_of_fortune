using UnityEngine;
using WheelOfFortune.Gameplay.Reward;

namespace WheelOfFortune.Gameplay.Revive
{
    [CreateAssetMenu(fileName = "ReviveCurrencyConfig", menuName = "WheelOfFortune/Revive/ReviveCurrencyConfig")]
    public class ReviveCurrencyConfig : ScriptableObject
    {
        [SerializeField] private RewardDefinition _reviveCurrencyRewardDefinition;
        [SerializeField] private int _startReviveCost = 25;
        [SerializeField] private int _reviveCostMultiplier = 2;

        public string ReviveCurrencyName => _reviveCurrencyRewardDefinition.Name;

        public int GetReviveCost(int reviveCount)
        {
            return _startReviveCost * (int)Mathf.Pow(_reviveCostMultiplier, reviveCount);
        }
    }
}
