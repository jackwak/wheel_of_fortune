using TMPro;
using UnityEngine.UI;

namespace WheelOfFortune.Gameplay.Wheel
{
    public interface IWheelCellContent
    {
        void Render(Image iconImage, TextMeshProUGUI countText);
        void Apply();
    }
}