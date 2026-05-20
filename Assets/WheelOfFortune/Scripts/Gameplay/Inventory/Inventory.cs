using System.Collections.Generic;
using UnityEngine;
using WheelOfFortune.Core.EventSystem;
using WheelOfFortune.Core.SaveSystem;
using WheelOfFortune.Gameplay.Exit.Events;
using WheelOfFortune.Gameplay.Inventory.Events;
using WheelOfFortune.Gameplay.RewardMoveEffectManager;
using Zenject;

namespace WheelOfFortune.Gameplay.Inventory
{
    public class Inventory : MonoBehaviour
    {
        private InventoryData _inventoryData;

        private ISaveManager _saveManager;
        private IEventBus _eventBus;

        private const string InventoryDataKey = "InventoryData";

        [Inject]
        public void Construct(ISaveManager saveManager, IEventBus eventBus)
        {
            _saveManager = saveManager;
            _eventBus = eventBus;
            InitializeInventory();
        }

        void OnEnable()
        {
            _eventBus.Subscribe<OnExitGameEvent>(OnExitGame);
            _eventBus.Subscribe<OnRewardEffectArrivedEvent>(OnRewardEffectArrivedEvent);
        }

        void OnDisable()
        {
            _eventBus.UnSubscribe<OnExitGameEvent>(OnExitGame);
            _eventBus.UnSubscribe<OnRewardEffectArrivedEvent>(OnRewardEffectArrivedEvent);
        }

        public bool HasItem(string itemName, int requiredCount)
        {
            if (_inventoryData.Items == null)
            {
                return false;
            }

            int count = 0;
            foreach (var item in _inventoryData.Items)
            {
                if (item.Name == itemName)
                {
                    count += item.Count;
                }
            }
            return count >= requiredCount;
        }

        public int GetItemCount(string itemName)
        {
            if (_inventoryData.Items == null)
            {
                return 0;
            }

            int count = 0;
            foreach (var item in _inventoryData.Items)
            {
                if (item.Name == itemName)
                {
                    count += item.Count;
                }
            }
            return count;
        }

        public bool TrySpend(string itemName, int requiredCount)
        {
            if (requiredCount <= 0)
            {
                return true;
            }

            if (!HasItem(itemName, requiredCount))
            {
                return false;
            }

            int remaining = requiredCount;
            for (int i = _inventoryData.Items.Count - 1; i >= 0 && remaining > 0; i--)
            {
                var item = _inventoryData.Items[i];
                if (item.Name != itemName)
                {
                    continue;
                }

                if (item.Count <= remaining)
                {
                    remaining -= item.Count;
                    _inventoryData.Items.RemoveAt(i);
                }
                else
                {
                    item.Count -= remaining;
                    _inventoryData.Items[i] = item;
                    remaining = 0;
                }
            }
            _eventBus.Publish(new OnInventoryChangedEvent());
            return true;
        }

        private void OnExitGame(OnExitGameEvent e)
        {
            CommitPendingRewards();
            SaveInventory();
        }

        private void CommitPendingRewards()
        {
            if (_inventoryData.PendingItems.Count == 0)
            {
                return;
            }

            foreach (var item in _inventoryData.PendingItems)
            {
                _inventoryData.Items.Add(item);
            }

            _inventoryData.PendingItems.Clear();
            _eventBus.Publish(new OnInventoryChangedEvent());
        }

        private void InitializeInventory()
        {
            _inventoryData = _saveManager.Load(InventoryDataKey, new InventoryData());

            if (_inventoryData.Items == null)
            {
                _inventoryData.Items = new List<ItemData>();
            }

            if (_inventoryData.PendingItems == null)
            {
                _inventoryData.PendingItems = new List<ItemData>();
            }
        }

        private void SaveInventory()
        {
            _saveManager.Save(InventoryDataKey, _inventoryData);
        }

        private void AddPendingReward(ItemData itemData)
        {
            _inventoryData.PendingItems.Add(itemData);
        }

        private void OnRewardEffectArrivedEvent(OnRewardEffectArrivedEvent e)
        {
            if (e.RewardData == null)
            {
                return;
            }

            AddPendingReward(new ItemData(e.RewardData.Definition.Name, e.RewardData.Count));
        }
    }
}