using UnityEngine;

namespace WheelOfFortune.Gameplay.Reward
{
    [CreateAssetMenu(fileName = "RewardDefinition", menuName = "WheelOfFortune/Gameplay/Reward/RewardDefinition")]
    public class RewardDefinition : ScriptableObject
    {
        public string Name;
        public Sprite Icon;
    }
}
