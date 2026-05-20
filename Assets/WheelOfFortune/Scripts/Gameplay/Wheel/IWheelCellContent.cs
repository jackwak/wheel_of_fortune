using TMPro;
using UnityEngine.UI;
using WheelOfFortune.Gameplay.Reward;

namespace WheelOfFortune.Gameplay.Wheel
{
    public interface IWheelCellContent
    {
        void Render(Image iconImage, TextMeshProUGUI countText);
        RewardData GetRewardData();
    }
}