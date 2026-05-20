using UnityEngine;
using DG.Tweening;

namespace WheelOfFortune.Gameplay.RewardMoveEffectManager
{
    [CreateAssetMenu(fileName = "RewardMoveEffectConfig", menuName = "WheelOfFortune/Gameplay/Reward Move Effect Config")]
    public class RewardMoveEffectConfig : ScriptableObject
    {
        [Header("Spawn")]
        [SerializeField] private float _spawnRadius = 50f;
        [SerializeField] private float _spawnDelayMin = 0.05f;
        [SerializeField] private float _spawnDelayMax = 0.1f;
        [SerializeField] private int _maxEffectCount = 10;

        [Header("Move")]
        [SerializeField] private float _moveDuration = 0.8f;
        [SerializeField] private Ease _moveEase = Ease.InBack;

        public float SpawnRadius => _spawnRadius;
        public float SpawnDelayMin => _spawnDelayMin;
        public float SpawnDelayMax => _spawnDelayMax;
        public float MoveDuration => _moveDuration;
        public Ease MoveEase => _moveEase;
        public int MaxEffectCount => _maxEffectCount;
    }
}