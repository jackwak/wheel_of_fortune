using UnityEngine;

namespace WheelOfFortune.Utils
{
    public interface IUIMover
    {
        void Move(RectTransform rectTransform, Vector2 offset, float duration);
        void MoveAll(RectTransform[] rectTransforms, Vector2 offset, float duration);
    }
}