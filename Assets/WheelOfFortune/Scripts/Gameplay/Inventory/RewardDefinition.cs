using UnityEngine;
using WheelOfFortune.Enums;

[CreateAssetMenu(fileName = "RewardDefinition", menuName = "WheelOfFortune/Rewards/RewardDefinition")]
public class RewardDefinition : ScriptableObject
{
    public string Name;
    public Sprite Icon;
}