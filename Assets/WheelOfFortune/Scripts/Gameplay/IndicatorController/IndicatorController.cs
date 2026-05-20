using DG.Tweening;
using UnityEngine;
using WheelOfFortune.Core.EventSystem;
using WheelOfFortune.Gameplay.SpinController.Events;
using WheelOfFortune.Gameplay.Wheel;
using Zenject;
using WheelOfFortune.Gameplay.CollectedReward;
using WheelOfFortune.Gameplay.IndicatorController.Events;
using WheelOfFortune.Gameplay.Reward;

namespace WheelOfFortune.Gameplay.IndicatorController
{
    public class IndicatorController : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private IndicatorControllerConfig _config;
        [SerializeField] private LayerMask _triggerLayerMask;
        [SerializeField] private Transform _objectToRotate;
        private IEventBus _eventBus;
        private CollectedRewardManager _collectedRewardManager;

        private Tween _rotationTween;
        private Vector3 _initialRotation;
        private Collider2D _lastTriggeredCollider;

        [Inject]
        public void Construct(IEventBus eventBus, CollectedRewardManager collectedRewardManager)
        {
            _eventBus = eventBus;
            _collectedRewardManager = collectedRewardManager;
        }

        private void Awake()
        {
            _initialRotation = _objectToRotate.localEulerAngles;
        }

        void OnEnable()
        {
            _eventBus.Subscribe<OnSpinCompleteEvent>(OnSpinComplete);
        }

        void OnDisable()
        {
            _eventBus.UnSubscribe<OnSpinCompleteEvent>(OnSpinComplete);
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (((1 << collision.gameObject.layer) & _triggerLayerMask) == 0) return;
            if (!collision.TryGetComponent(out WheelCellDisplay _)) return;
            if (collision == _lastTriggeredCollider) return;

            _lastTriggeredCollider = collision;
            RotateIndicator();
        }

        private void RotateIndicator()
        {
            _rotationTween?.Kill();

            _rotationTween = DOTween.Sequence().Append(_objectToRotate.DOLocalRotate(new Vector3(0, 0, _config.RotationAngle * _config.SpinDirectionMultiplier), _config.RotationDuration
                ).SetEase(_config.RotationEase)).Append(_objectToRotate.DOLocalRotate(_initialRotation, _config.RotationDuration
                ).SetEase(Ease.OutQuad))
                .OnComplete(() =>
                {
                    _rotationTween = null;
                });
        }

        private void OnSpinComplete(OnSpinCompleteEvent eventData)
        {
            RewardData rewardData = _lastTriggeredCollider.GetComponent<WheelCellDisplay>().GetContent().GetRewardData();
            
            if (rewardData != null)
            {
                Vector3 targetPosition = _collectedRewardManager.GetOrCreateDisplayPosition(rewardData);

                _eventBus.Publish(new OnStartCollectingRewardEvent(rewardData, _lastTriggeredCollider.transform.position, targetPosition));
                _lastTriggeredCollider = null;
                return;
            }
            else
            {
                _eventBus.Publish(new OnCollectBombEvent());
                _lastTriggeredCollider = null;
            }
        }
    }
}