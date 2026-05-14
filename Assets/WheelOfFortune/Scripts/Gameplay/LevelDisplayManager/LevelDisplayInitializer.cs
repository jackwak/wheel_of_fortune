using UnityEngine;

namespace WheelOfFortune.Gameplay.LevelDisplayManager
{
    public class LevelDisplayInitializer : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] private LevelDisplayConfig _config;
        [SerializeField] private LevelDisplayNumberConfig _numberConfig;

        [Header("References")]
        [SerializeField] private LevelDisplayNumberBehaviour _levelNumberPrefab;
        [SerializeField] private Transform _container;

        public void Initialize(LevelDisplayNumberController numberController)
        {
            LevelDisplayNumberBehaviour[] levelNumbers = SpawnLevelNumbers();
            ArrangeLevelNumbers(levelNumbers);
            numberController.Initialize(levelNumbers);
        }

        private void ArrangeLevelNumbers(LevelDisplayNumberBehaviour[] levelNumbers)
        {
            for (int i = 0; i < levelNumbers.Length; i++)
            {
                RectTransform rectTransform = levelNumbers[i].transform as RectTransform;
                rectTransform.anchoredPosition = new Vector3(i * _numberConfig.StepSize, 0, 0);
            }
        }

        private LevelDisplayNumberBehaviour[] SpawnLevelNumbers()
        {
            LevelDisplayNumberBehaviour[] levelNumbers = new LevelDisplayNumberBehaviour[_config.MaxLevel];

            for (int i = 0; i < _config.MaxLevel; i++)
            {
                levelNumbers[i] = Instantiate(_levelNumberPrefab, _container);
                levelNumbers[i].Initialize(new LevelDisplayNumberData(i + 1));
            }

            return levelNumbers;
        }
    }
}