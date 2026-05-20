using UnityEngine;
using WheelOfFortune.Core.EventSystem;
using WheelOfFortune.Events;
using WheelOfFortune.Gameplay.Inventory.Events;
using WheelOfFortune.Gameplay.Reward;
using Zenject;

namespace WheelOfFortune.Gameplay.CurrencyDisplayController
{
    public class CurrencyDisplayController : MonoBehaviour
    {
        [SerializeField] private CurrencyDisplay _currencyDisplay;
        [SerializeField] private RewardDefinition _currencyRewardDefinition;

        private IEventBus _eventBus;
        private Inventory.Inventory _inventory;

        [Inject]
        public void Construct(IEventBus eventBus, Inventory.Inventory inventory)
        {
            _eventBus = eventBus;
            _inventory = inventory;
        }

        void OnEnable()
        {
            _eventBus.Subscribe<OnGameStartEvent>(OnGameStart);
            _eventBus.Subscribe<OnInventoryChangedEvent>(OnInventoryChanged);
        }

        void OnDisable()
        {
            _eventBus.UnSubscribe<OnGameStartEvent>(OnGameStart);
            _eventBus.UnSubscribe<OnInventoryChangedEvent>(OnInventoryChanged);
        }

        public void UpdateCurrency(int newAmount)
        {
            _currencyDisplay.UpdateCurrency(newAmount);
        }

        private void OnGameStart(OnGameStartEvent eventData)
        {
            RefreshDisplay();
        }

        private void OnInventoryChanged(OnInventoryChangedEvent _)
        {
            RefreshDisplay();
        }

        private void RefreshDisplay()
        {
            int currencyCount = _inventory.GetItemCount(_currencyRewardDefinition.Name);
            UpdateCurrency(currencyCount);
        }
    }
}