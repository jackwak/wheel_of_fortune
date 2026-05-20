using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

namespace WheelOfFortune.Gameplay.RewardMoveEffectManager
{
    public class RewardMoveEffectBehaviour : MonoBehaviour
    {
        [SerializeField] private Image _rewardImage;

        private RectTransform _rectTransform;
        private Action _onMoveComplete;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public void Spawn(Sprite rewardSprite, Vector2 anchoredPosition)
        {
            _rewardImage.sprite = rewardSprite;
            _rectTransform.anchoredPosition = anchoredPosition;
        }

        public void Move(Vector2 endAnchoredPosition, Action onComplete = null, float duration = 0.8f, Ease ease = Ease.InBack)
        {
            _onMoveComplete = onComplete;

            _rectTransform
                .DOAnchorPos(endAnchoredPosition, duration)
                .SetEase(ease)
                .OnComplete(() => _onMoveComplete?.Invoke());
        }

        private void OnDisable()
        {
            _rectTransform.DOKill();
        }
    }
}