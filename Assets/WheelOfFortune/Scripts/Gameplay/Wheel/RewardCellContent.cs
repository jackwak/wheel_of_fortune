using TMPro;
using UnityEngine.UI;
using WheelOfFortune.Gameplay.Reward;

namespace WheelOfFortune.Gameplay.Wheel
{
    public class RewardCellContent : IWheelCellContent
    {
        private readonly RewardData _rewardData;

        public RewardCellContent(RewardData rewardData) => _rewardData = rewardData;

        public void Render(Image iconImage, TextMeshProUGUI countText)
        {
            iconImage.sprite = _rewardData.Definition.Icon;
            countText.text = $"x{_rewardData.Count}";
        }

        public RewardData GetRewardData() => _rewardData;
    }
}