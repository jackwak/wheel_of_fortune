using System.Collections;
using UnityEngine;
using WheelOfFortune.Utils.UIMover;
using WheelOfFortune.Gameplay.LevelDisplayManager.Config;
using Zenject;

namespace WheelOfFortune.Gameplay.LevelDisplayManager
{
    public class LevelDisplayNumberController : MonoBehaviour
    {
        [Header("References")]
        private IUIMover _uiMover;
        private LevelDisplayNumberBehaviour[] _levelNumbers;
        private RectTransform _viewportRect;

        [Header("Config")]
        [SerializeField] private LevelDisplayNumberConfig _numberConfig;

        [Header("Data")]
        private int _nextLevelToAssign;

        [Inject]
        public void Construct(IUIMover uiMover)
        {
            _uiMover = uiMover;
        }

        public void Initialize(LevelDisplayNumberBehaviour[] levelNumbers, RectTransform viewportRect, int startLevel)
        {
            _levelNumbers = levelNumbers;
            _viewportRect = viewportRect;
            _nextLevelToAssign = startLevel + levelNumbers.Length;
        }

        public void ScrollNumbers()
        {
            RectTransform[] rectTransforms = new RectTransform[_levelNumbers.Length];

            for (int i = 0; i < _levelNumbers.Length; i++)
            {
                rectTransforms[i] = _levelNumbers[i].transform as RectTransform;
            }

            _uiMover.MoveAll(rectTransforms, new Vector2(-_numberConfig.StepSize, 0), _numberConfig.ScrollDuration);
            StartCoroutine(RecycleAfterScroll());
        }

        private IEnumerator RecycleAfterScroll()
        {
            yield return new WaitForSeconds(_numberConfig.ScrollDuration);

            for (int i = 0; i < _levelNumbers.Length; i++)
            {
                RectTransform rectTransform = _levelNumbers[i].transform as RectTransform;

                if (IsFullyOutsideLeft(rectTransform, _viewportRect))
                {
                    MoveToEnd(rectTransform);

                    _levelNumbers[i].Initialize(new LevelDisplayNumberData(_nextLevelToAssign));
                    _nextLevelToAssign++;
                }
            }
        }

        private void MoveToEnd(RectTransform target)
        {
            RectTransform rightMost = GetRightMostRect();

            Vector2 newPosition = target.anchoredPosition;
            newPosition.x = rightMost.anchoredPosition.x + _numberConfig.StepSize;

            target.anchoredPosition = newPosition;
        }

        private RectTransform GetRightMostRect()
        {
            RectTransform rightMost = _levelNumbers[0].transform as RectTransform;

            for (int i = 1; i < _levelNumbers.Length; i++)
            {
                RectTransform rect = _levelNumbers[i].transform as RectTransform;

                if (rect.anchoredPosition.x > rightMost.anchoredPosition.x)
                {
                    rightMost = rect;
                }
            }

            return rightMost;
        }

        private bool IsFullyOutsideLeft(RectTransform target, RectTransform viewport)
        {
            Vector3[] targetCorners = new Vector3[4];
            Vector3[] viewportCorners = new Vector3[4];

            target.GetWorldCorners(targetCorners);
            viewport.GetWorldCorners(viewportCorners);

            float targetRightX = targetCorners[2].x;
            float viewportLeftX = viewportCorners[0].x;

            return targetRightX < viewportLeftX;
        }
    }
}