using System.Collections.Generic;
using UnityEngine;
using WheelOfFortune.Core.EventBus;
using WheelOfFortune.Enums;
using WheelOfFortune.Events;
using WheelOfFortune.Utils.RankDeterminer;
using Zenject;

namespace WheelOfFortune.Gameplay.Wheel
{
    public class WheelController : MonoBehaviour
    {
        [SerializeField] private WheelInitializer _initializer;
        [SerializeField] private WheelConfig _config;

        private IEventBus _eventBus;
        private IRewardDataProvider _rewardDataProvider;
        private RankDeterminer _rankDeterminer;

        [Inject]
        public void Construct(IEventBus eventBus, IRewardDataProvider rewardCatalogProvider, RankDeterminer rankDeterminer)
        {
            _eventBus = eventBus;
            _rewardDataProvider = rewardCatalogProvider;
            _rankDeterminer = rankDeterminer;
        }

        void OnEnable()
        {
            _eventBus.Subscribe<OnGameStartEvent>(OnGameStart);
            //_eventBus.Subscribe<LevelChangedEvent>(OnLevelChanged);
        }

        void OnDisable()
        {
            _eventBus.UnSubscribe<OnGameStartEvent>(OnGameStart);
            //_eventBus.UnSubscribe<LevelChangedEvent>(OnLevelChanged);
        }

        private void OnGameStart(OnGameStartEvent eventData)
        {
            _initializer.InitializeWheelCell(_config);
            InitializeWheel(eventData.StartLevel);
        }

        //TODO: subscribe after reward movement is done, currently subscribing to OnGameStartEvent for testing purposes
        private void OnLevelChanged(LevelChangedEvent eventData)
        {
            InitializeWheel(eventData.NewLevel);
        }

        private void InitializeWheel(int startLevel)
        {
            Rank rank = _rankDeterminer.DetermineRank(startLevel);
            int rewardCount = _config.SliceCount;
            int bombCount = 0;

            foreach (var item in _config.BombCountByRank)
            {
                if (item.Rank == rank)
                {
                    bombCount = item.BombCount;
                    rewardCount -= bombCount;
                    break;
                }
            }

            RewardData[] rewardDatas = _rewardDataProvider.GetRandomRewardDataByRank(rank, rewardCount);

            IWheelCellContent[] contents = BuildContents(rewardDatas, bombCount);

            _initializer.InitializeWheel(contents);
        }

        private IWheelCellContent[] BuildContents(RewardData[] rewardDatas, int bombCount)
        {
            var contents = new List<IWheelCellContent>();

            foreach (var rewardData in rewardDatas)
                contents.Add(new RewardCellContent(rewardData));

            for (int i = 0; i < bombCount; i++)
                contents.Add(new BombCellContent(_config.BombSprite));

            Shuffle(contents);

            return contents.ToArray();
        }

        private void Shuffle<T>(List<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }
    }
}