using DG.Tweening;
using UnityEngine;

namespace WheelOfFortune.Utils.UIMover
{
    public class UIDoTweenMover : IUIMover
    {
        public void Move(RectTransform rectTransform, Vector2 offset, float duration)
        {
            rectTransform.DOAnchorPos(rectTransform.anchoredPosition + offset, duration);
        }

        public void MoveAll(RectTransform[] rectTransforms, Vector2 offset, float duration)
        {
            foreach (RectTransform rectTransform in rectTransforms)
            {
                rectTransform.DOAnchorPos(rectTransform.anchoredPosition + offset, duration);
            }
        }
    }
}