using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using WheelOfFortune.Core.EventSystem;
using WheelOfFortune.Core.ObjectPool;
using WheelOfFortune.Gameplay.IndicatorController.Events;
using WheelOfFortune.Gameplay.Reward;
using Zenject;

namespace WheelOfFortune.Gameplay.RewardMoveEffectManager
{
    public class RewardMoveEffectManager : MonoBehaviour
    {
        [SerializeField] private GameObject _rewardEffectPrefab;
        [SerializeField] private RectTransform _canvasRectTransform;
        [SerializeField] private Transform _effectPoolParent;
        [SerializeField] private RewardMoveEffectConfig _config;

        private PoolManager _effectPool;
        private IEventBus _eventBus;

        [Inject]
        public void Construct(PoolManager effectPool, IEventBus eventBus)
        {
            _effectPool = effectPool;
            _eventBus = eventBus;
            effectPool.SetParent(_rewardEffectPrefab, _effectPoolParent);
        }

        private void OnEnable()
        {
            _eventBus.Subscribe<OnStartCollectingRewardEvent>(OnStartCollectingReward);
        }

        private void OnDisable()
        {
            _eventBus.UnSubscribe<OnStartCollectingRewardEvent>(OnStartCollectingReward);
        }

        private void OnStartCollectingReward(OnStartCollectingRewardEvent eventData)
        {
            PlayEffect(eventData.RewardData, eventData.StartPosition, eventData.TargetPosition);
        }

        public void PlayEffect(RewardData rewardData, Vector3 worldStartPosition, Vector3 worldEndPosition)
        {
            PlayEffectAsync(rewardData, worldStartPosition, worldEndPosition).Forget();
        }

        private async UniTaskVoid PlayEffectAsync(RewardData rewardData, Vector3 worldStartPosition, Vector3 worldEndPosition)
        {
            Vector2 spawnCenter = ScreenToAnchoredPosition(worldStartPosition);
            Vector2 targetPosition = ScreenToAnchoredPosition(worldEndPosition);

            int effectCount = Mathf.Min(rewardData.Count, _config.MaxEffectCount);
            List<RewardMoveEffectBehaviour> effects = new(effectCount);

            for (int i = 0; i < effectCount; i++)
            {
                RewardMoveEffectBehaviour effect = SpawnEffect(rewardData.Definition.Icon, spawnCenter);
                effects.Add(effect);

                float delay = UnityEngine.Random.Range(_config.SpawnDelayMin, _config.SpawnDelayMax);
                await UniTask.Delay(TimeSpan.FromSeconds(delay));
            }

            int remaining = effects.Count;
            foreach (RewardMoveEffectBehaviour effect in effects)
            {
                effect.Move(targetPosition, () =>
                {
                    ReleaseEffect(effect);

                    remaining--;
                    if (remaining == 0)
                        _eventBus.Publish(new OnRewardEffectArrivedEvent(rewardData));
                });
            }
        }

        private RewardMoveEffectBehaviour SpawnEffect(Sprite icon, Vector2 center)
        {
            Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * _config.SpawnRadius;
            Vector2 spawnPosition = center + randomOffset;

            GameObject effectObject = _effectPool.Get(_rewardEffectPrefab, Vector3.zero, Quaternion.identity);
            RewardMoveEffectBehaviour effect = effectObject.GetComponent<RewardMoveEffectBehaviour>();
            effect.Spawn(icon, spawnPosition);

            return effect;
        }

        private void ReleaseEffect(RewardMoveEffectBehaviour effect)
        {
            _effectPool.Release(_rewardEffectPrefab, effect.gameObject);
        }

        private Vector2 ScreenToAnchoredPosition(Vector2 screenPosition)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRectTransform, screenPosition, null, out Vector2 localPoint);
            return localPoint;
        }
    }
}
