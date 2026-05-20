using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WheelOfFortune.Gameplay.Reward;

namespace WheelOfFortune.Gameplay.CollectedReward
{
    public class CollectedRewardDisplay : MonoBehaviour
    {
        private RewardData _content;
        private bool _isInitialized;

        [SerializeField] private Image _iconImage;
        [SerializeField] private TextMeshProUGUI _countText;

        public RewardData GetContent() => _content;
        public bool IsInitialized => _isInitialized;

        void OnEnable()
        {
            ResetText();
        }

        public void Initialize(RewardData content)
        {
            _content = content;
            _isInitialized = true;

            _iconImage.sprite = content.Definition.Icon;
            _countText.text = content.Count.ToString();
        }

        public void InitializeWithoutCount(RewardData content)
        {
            _content = content;
            _isInitialized = true;

            _iconImage.sprite = content.Definition.Icon;
            _countText.text = string.Empty;
        }

        private void ResetText()
        {
            _countText.text = string.Empty;
        }
    }
}