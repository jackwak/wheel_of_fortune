using UnityEngine;
using WheelOfFortune.Core.EventSystem;
using WheelOfFortune.Events;
using WheelOfFortune.Gameplay.LevelFailedPanelController;
using WheelOfFortune.Gameplay.Revive.Events;
using Zenject;

namespace WheelOfFortune.Gameplay.Revive
{
    public class ReviveService : MonoBehaviour
    {
        [SerializeField] private ReviveCurrencyConfig _config;

        private IEventBus _eventBus;
        private Inventory.Inventory _inventory;

        private int _reviveCount;

        [Inject]
        public void Construct(IEventBus eventBus, Inventory.Inventory inventory)
        {
            _eventBus = eventBus;
            _inventory = inventory;
        }

        private void OnEnable()
        {
            _eventBus.Subscribe<OnReviveRequestedEvent>(OnReviveRequested);
            _eventBus.Subscribe<OnGameStartEvent>(OnGameStart);
        }

        private void OnDisable()
        {
            _eventBus.UnSubscribe<OnReviveRequestedEvent>(OnReviveRequested);
            _eventBus.UnSubscribe<OnGameStartEvent>(OnGameStart);
        }

        public int GetCurrentReviveCost()
        {
            return _config.GetReviveCost(_reviveCount);
        }

        private void OnReviveRequested(OnReviveRequestedEvent _)
        {
            int cost = _config.GetReviveCost(_reviveCount);
            if (_inventory.TrySpend(_config.ReviveCurrencyName, cost))
            {
                _reviveCount++;
                _eventBus.Publish(new OnPlayerRevivedEvent());
            }
            else
            {
                Debug.Log("Not enough Revive Tokens!");
            }
        }

        private void OnGameStart(OnGameStartEvent _)
        {
            _reviveCount = 0;
        }
    }
}
