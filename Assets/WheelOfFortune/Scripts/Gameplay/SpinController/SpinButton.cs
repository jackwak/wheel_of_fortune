using WheelOfFortune.Core.EventSystem;
using WheelOfFortune.Events;
using WheelOfFortune.Gameplay.LevelFailedPanelController;
using WheelOfFortune.Gameplay.SpinController.Events;
using WheelOfFortune.Gameplay.SpinController.States;
using WheelOfFortune.Utils.BaseButton;
using Zenject;

namespace WheelOfFortune.Gameplay.SpinController
{
    public class SpinButton : BaseButton
    {
        private readonly ISpinButtonState _interactable = new InteractableState();
        private readonly ISpinButtonState _nonInteractable = new NonInteractableState();

        private IEventBus _eventBus;
        private ISpinButtonState _currentState;

        [Inject]
        private void Construct(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _eventBus.Subscribe<SpinButtonClickedEvent>(OnSpinButtonClicked);
            _eventBus.Subscribe<LevelChangedEvent>(LevelChangedEvent);
            _eventBus.Subscribe<OnGameStartEvent>(OnGameStart);
            _eventBus.Subscribe<OnPlayerRevivedEvent>(OnPlayerRevived);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _eventBus.UnSubscribe<SpinButtonClickedEvent>(OnSpinButtonClicked);
            _eventBus.UnSubscribe<LevelChangedEvent>(LevelChangedEvent);
            _eventBus.UnSubscribe<OnGameStartEvent>(OnGameStart);
            _eventBus.UnSubscribe<OnPlayerRevivedEvent>(OnPlayerRevived);
        }

        protected override void OnButtonClicked()
        {
            _eventBus.Publish(new SpinButtonClickedEvent());
        }

        public void ApplyInteractable(bool value)
        {
            SetInteractable(value);
        }

        private void OnSpinButtonClicked(SpinButtonClickedEvent _)
        {
            ChangeState(_nonInteractable);
        }

        private void LevelChangedEvent(LevelChangedEvent _)
        {
            ChangeState(_interactable);
        }

        private void OnGameStart(OnGameStartEvent _)
        {
            ChangeState(_interactable);
        }

        private void OnPlayerRevived(OnPlayerRevivedEvent _)
        {
            ChangeState(_interactable);
        }

        private void ChangeState(ISpinButtonState next)
        {
            _currentState?.Exit(this);
            _currentState = next;
            _currentState.Enter(this);
        }
    }
}
