using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WheelOfFortune.Core.EventSystem;
using WheelOfFortune.Core.ObjectPool;
using WheelOfFortune.Gameplay.Reward;
using WheelOfFortune.Gameplay.RewardMoveEffectManager;
using Zenject;

namespace WheelOfFortune.Gameplay.CollectedReward
{
    public class CollectedRewardManager : MonoBehaviour
    {
        [SerializeField] private GameObject _collectedRewardDisplayPrefab;
        [SerializeField] private Transform _displayPoolParent;

        private List<CollectedRewardDisplay> _rewardDisplays = new();
        private PoolManager _displayPool;
        private IEventBus _eventBus;

        [Inject]
        public void Construct(PoolManager displayPool, IEventBus eventBus)
        {
            _displayPool = displayPool;
            _eventBus = eventBus;
        }

        void OnEnable()
        {
            _eventBus.Subscribe<OnRewardEffectArrivedEvent>(OnRewardEffectArrived);
        }

        void OnDisable()
        {
            _eventBus.UnSubscribe<OnRewardEffectArrivedEvent>(OnRewardEffectArrived);
        }

        private void UpdateDisplayCount(CollectedRewardDisplay display, RewardData content)
        {
            int currentCount = display.IsInitialized ? display.GetContent().Count : 0;
            display.Initialize(new RewardData(content.Definition, currentCount + content.Count));
        }

        private CollectedRewardDisplay GetFromPool()
        {
            return _displayPool.Get(_collectedRewardDisplayPrefab, _displayPoolParent, Vector3.zero, Quaternion.identity, false).GetComponent<CollectedRewardDisplay>();
        }

        private CollectedRewardDisplay GetOrCreateDisplay(RewardData content)
        {
            foreach (var display in _rewardDisplays)
            {
                if (display.IsInitialized && display.GetContent().Definition.Name == content.Definition.Name)
                    return display;
            }

            CollectedRewardDisplay newDisplay = GetFromPool();
            newDisplay.InitializeWithoutCount(new RewardData(content.Definition, 0));
            _rewardDisplays.Add(newDisplay);
            return newDisplay;
        }

        private void OnRewardEffectArrived(OnRewardEffectArrivedEvent eventData)
        {
            AddReward(eventData.RewardData);
        }

        public void AddReward(RewardData content)
        {
            CollectedRewardDisplay display = GetOrCreateDisplay(content);
            UpdateDisplayCount(display, content);
        }

        public Vector3 GetOrCreateDisplayPosition(RewardData content)
        {
            CollectedRewardDisplay display = GetOrCreateDisplay(content);

            if (display.transform.parent is RectTransform parentRect)
                LayoutRebuilder.ForceRebuildLayoutImmediate(parentRect);

            return display.transform.position;
        }
    }
}