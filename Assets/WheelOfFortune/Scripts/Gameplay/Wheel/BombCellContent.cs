using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace WheelOfFortune.Gameplay.Wheel
{
    public class BombCellContent : IWheelCellContent
    {
        private readonly Sprite _bombSprite;

        public BombCellContent(Sprite bombSprite) => _bombSprite = bombSprite;

        public void Render(Image iconImage, TextMeshProUGUI countText)
        {
            iconImage.sprite = _bombSprite;
            countText.text = "";
        }

        public void Apply() { }
    }
}