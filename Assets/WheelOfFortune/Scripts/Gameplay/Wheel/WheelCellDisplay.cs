using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace WheelOfFortune.Gameplay.Wheel
{
    public class WheelCellDisplay : MonoBehaviour
    {
        [SerializeField] private Image _iconImage;
        [SerializeField] private TextMeshProUGUI _countText;

        private IWheelCellContent _content;

        public void Initialize(IWheelCellContent content)
        {
            _content = content;
            Render(content);
        }

        private void Render(IWheelCellContent content)
        {
            content.Render(_iconImage, _countText);
        }

        public IWheelCellContent GetContent() => _content;
    }
}