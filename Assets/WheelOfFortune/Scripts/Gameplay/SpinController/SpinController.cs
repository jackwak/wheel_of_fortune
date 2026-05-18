using UnityEngine;
using WheelOfFortune.Core.EventBus;
using WheelOfFortune.Events;
using WheelOfFortune.Gameplay.SpinController.Events;
using WheelOfFortune.Utils.ObjectSpinner;
using Zenject;

namespace WheelOfFortune.Gameplay.SpinController
{
    public class SpinController : MonoBehaviour
    {
        [Header(" UI Elements ")]
        [SerializeField] private Transform _objectToSpin;


        [Header(" Dependencies ")]
        [SerializeField] private SpinnerConfig _spinnerConfig;
        // TODO: Wheel segments should be determined by the wheel design, not hardcoded
        [SerializeField] private int _wheelSegments = 8;
        private IObjectSpinner _objectSpinner;
        private IEventBus _eventBus;

        [Inject]
        public void Construct(IObjectSpinner objectSpinner, IEventBus eventBus)
        {
            _objectSpinner = objectSpinner;
            _eventBus = eventBus;
            _objectSpinner.Initialize(_spinnerConfig);
        }

        private void OnEnable()
        {
            _eventBus.Subscribe<SpinButtonClickedEvent>(OnSpinButtonClicked);
        }

        private void OnDisable()
        {
            _eventBus.UnSubscribe<SpinButtonClickedEvent>(OnSpinButtonClicked);
        }

        private void OnSpinButtonClicked(SpinButtonClickedEvent eventData)
        {
            int randomSegment = Random.Range(0, _wheelSegments);
            _objectSpinner.SpinObject(_objectToSpin, _wheelSegments, randomSegment, OnSpinComplete);
        }

        // TODO: Level management should be handled by a dedicated GameManager or LevelManager, not directly in the SpinController
        int level = 1;

        public void OnSpinComplete()
        {
            _eventBus.Publish(new LevelChangedEvent(level++));
        }
    }
}