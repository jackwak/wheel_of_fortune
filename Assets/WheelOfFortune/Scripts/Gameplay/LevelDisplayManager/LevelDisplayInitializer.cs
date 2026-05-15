using UnityEngine;
using Zenject;

namespace WheelOfFortune.Gameplay.LevelDisplayManager
{
    public class LevelDisplayInitializer : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] private LevelDisplayConfig _config;
        [SerializeField] private LevelDisplayNumberConfig _numberConfig;

        [Header("References")]
        [SerializeField] private LevelDisplayNumberIndicatorBehaviour _numberIndicatorBehaviour;
        [SerializeField] private LevelDisplayNumberBehaviour _levelNumberPrefab;
        [SerializeField] private Transform _container;
        private DiContainer _diContainer;

        [Inject]
        public void Construct(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }

        public void Initialize(LevelDisplayNumberController numberController)
        {
            LevelDisplayNumberBehaviour[] levelNumbers = SpawnLevelNumbers();
            ArrangeLevelNumbers(levelNumbers);
            numberController.Initialize(levelNumbers);
            _numberIndicatorBehaviour.Initialize(_config.StartLevel);
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
                int level = _config.StartLevel + i;
                
                levelNumbers[i] = _diContainer.InstantiatePrefabForComponent<LevelDisplayNumberBehaviour>(_levelNumberPrefab, _container);
                levelNumbers[i].Initialize(new LevelDisplayNumberData(level));
            }

            return levelNumbers;
        }
    }
}