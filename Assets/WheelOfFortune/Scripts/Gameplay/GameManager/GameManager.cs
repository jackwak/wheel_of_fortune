using UnityEngine;
using UnityEngine.SceneManagement;
using WheelOfFortune.Config;
using WheelOfFortune.Core.EventSystem;
using WheelOfFortune.Events;
using WheelOfFortune.Gameplay.Exit.Events;
using WheelOfFortune.Gameplay.IndicatorController.Events;
using WheelOfFortune.Gameplay.LevelFlow.Events;
using WheelOfFortune.Gameplay.RewardMoveEffectManager;
using Zenject;

namespace WheelOfFortune.Gameplay.GameManager
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private LevelConfig _levelConfig;
        private IEventBus _eventBus;

        private int _startLevel;

        [Inject]
        public void Construct(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        void OnEnable()
        {
            _eventBus.Subscribe<OnRewardEffectArrivedEvent>(OnRewardEffectArrived);
            _eventBus.Subscribe<OnCollectBombEvent>(OnCollectBomb);
            _eventBus.Subscribe<OnGiveUpRequestedEvent>(OnGiveUpRequested);
            _eventBus.Subscribe<OnExitGameEvent>(OnExitGame);
        }

        void OnDisable()
        {
            _eventBus.UnSubscribe<OnRewardEffectArrivedEvent>(OnRewardEffectArrived);
            _eventBus.UnSubscribe<OnCollectBombEvent>(OnCollectBomb);
            _eventBus.UnSubscribe<OnGiveUpRequestedEvent>(OnGiveUpRequested);
            _eventBus.UnSubscribe<OnExitGameEvent>(OnExitGame);
        }

        void Start()
        {
            _startLevel = _levelConfig.StartLevel;
            _eventBus.Publish(new OnGameStartEvent(_startLevel));
        }

        private void OnRewardEffectArrived(OnRewardEffectArrivedEvent eventData)
        {
            _eventBus.Publish(new LevelChangedEvent(++_startLevel));
        }

        private void OnCollectBomb(OnCollectBombEvent _)
        {
            _eventBus.Publish(new OnLevelFailedEvent());
        }

        private void OnGiveUpRequested(OnGiveUpRequestedEvent _)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void OnExitGame(OnExitGameEvent _)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
