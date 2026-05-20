using UnityEngine.UI;
using UnityEngine;
using Zenject;
using WheelOfFortune.Utils.RankDeterminer;
using WheelOfFortune.Events;
using WheelOfFortune.Core.EventSystem;
using WheelOfFortune.Config;
using WheelOfFortune.Enums;

namespace WheelOfFortune.Gameplay.LevelDisplayManager
{
    public class LevelDisplayNumberIndicatorBehaviour : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private RankColorPalette _numberIndicatorRankPalette;

        [Header("Dependencies")]
        private IEventBus _eventBus;
        private RankDeterminer _rankDeterminer;

        [Inject]
        public void Construct(IEventBus eventBus, RankDeterminer rankDeterminer)
        {
            _eventBus = eventBus;
            _rankDeterminer = rankDeterminer;
        }

        void OnEnable()
        {
            _eventBus.Subscribe<LevelChangedEvent>(OnLevelNumberChanged);
        }

        void OnDisable()
        {
            _eventBus.UnSubscribe<LevelChangedEvent>(OnLevelNumberChanged);
        }

        public void Initialize(int startLevel)
        {
            SetBackgroundColor(GetColorByRank(GetRankByLevel(startLevel)));
        }

        private void OnLevelNumberChanged(LevelChangedEvent eventData)
        {
            SetBackgroundColor(GetColorByRank(GetRankByLevel(eventData.NewLevel)));
        }

        private void SetBackgroundColor(Color color)
        {
            _backgroundImage.color = color;
        }

        private Color GetColorByRank(Rank rank) => _numberIndicatorRankPalette.GetColor(rank);
        private Rank GetRankByLevel(int level) => _rankDeterminer.DetermineRank(level);
    }
}