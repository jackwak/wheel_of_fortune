using TMPro;
using UnityEngine.UI;

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

        public void Apply() { }
    }
}