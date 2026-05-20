using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WheelOfFortune.Core.EventSystem;
using WheelOfFortune.Enums;
using WheelOfFortune.Events;
using WheelOfFortune.Utils.RankDeterminer;
using Zenject;

namespace WheelOfFortune.Gameplay.Wheel.States
{
    public class WheelRankDisplay : MonoBehaviour
    {
        [SerializeField] private Image _wheelImage;
        [SerializeField] private Image _indicatorImage;
        [SerializeField] private TMP_Text _titleText;
        [SerializeField] private WheelRankVisualsConfig _visualsConfig;

        private IEventBus _eventBus;
        private RankDeterminer _rankDeterminer;

        private Dictionary<Rank, IWheelRankState> _states;
        private IWheelRankState _currentState;

        [Inject]
        public void Construct(IEventBus eventBus, RankDeterminer rankDeterminer)
        {
            _eventBus = eventBus;
            _rankDeterminer = rankDeterminer;
        }

        private void Awake()
        {
            _states = new Dictionary<Rank, IWheelRankState>
            {
                { Rank.Bronze, new BronzeWheelRankState() },
                { Rank.Silver, new SilverWheelRankState() },
                { Rank.Gold,   new GoldWheelRankState() }
            };
        }

        private void OnEnable()
        {
            _eventBus.Subscribe<OnGameStartEvent>(OnGameStart);
            _eventBus.Subscribe<LevelChangedEvent>(OnLevelChanged);
        }

        private void OnDisable()
        {
            _eventBus.UnSubscribe<OnGameStartEvent>(OnGameStart);
            _eventBus.UnSubscribe<LevelChangedEvent>(OnLevelChanged);
        }

        public void ApplyVisuals(Rank rank)
        {
            if (!_visualsConfig.TryGet(rank, out RankVisualData data))
            {
                return;
            }

            _wheelImage.sprite = data.WheelSprite;
            _indicatorImage.sprite = data.IndicatorSprite;
            _titleText.text = data.Title;
        }

        private void OnGameStart(OnGameStartEvent eventData)
        {
            SwitchToRankOf(eventData.StartLevel);
        }

        private void OnLevelChanged(LevelChangedEvent eventData)
        {
            SwitchToRankOf(eventData.NewLevel);
        }

        private void SwitchToRankOf(int level)
        {
            Rank rank = _rankDeterminer.DetermineRank(level);

            if (_currentState != null && _currentState.Rank == rank)
                return;

            ChangeState(_states[rank]);
        }

        private void ChangeState(IWheelRankState next)
        {
            _currentState?.Exit(this);
            _currentState = next;
            _currentState.Enter(this);
        }
    }
}
