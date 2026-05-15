using UnityEngine;
using WheelOfFortune.Utils.UIMover;
using Zenject;

namespace WheelOfFortune.Gameplay.LevelDisplayManager
{
    public class LevelDisplayNumberController : MonoBehaviour
    {
        [Header("References")]
        private IUIMover _uiMover;
        private LevelDisplayNumberBehaviour[] _levelNumbers;

        [Header("Config")]
        [SerializeField] private LevelDisplayNumberConfig _numberConfig;

        [Inject]
        public void Construct(IUIMover uiMover)
        {
            _uiMover = uiMover;
        }

        public void Initialize(LevelDisplayNumberBehaviour[] levelNumbers)
        {
            _levelNumbers = levelNumbers;
        }

        public void ScrollNumbers()
        {
            RectTransform[] rectTransforms = new RectTransform[_levelNumbers.Length];
            for (int i = 0; i < _levelNumbers.Length; i++)
            {
                rectTransforms[i] = _levelNumbers[i].transform as RectTransform;
            }

            _uiMover.MoveAll(rectTransforms, new Vector2(-_numberConfig.StepSize, 0), _numberConfig.ScrollDuration);
        }
    }
}