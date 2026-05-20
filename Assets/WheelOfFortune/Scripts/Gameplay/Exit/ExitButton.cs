using WheelOfFortune.Core.EventSystem;
using WheelOfFortune.Enums;
using WheelOfFortune.Events;
using WheelOfFortune.Gameplay.Exit.Events;
using WheelOfFortune.Gameplay.Exit.States;
using WheelOfFortune.Gameplay.IndicatorController.Events;
using WheelOfFortune.Gameplay.RewardMoveEffectManager;
using WheelOfFortune.Gameplay.SpinController.Events;
using WheelOfFortune.Utils.BaseButton;
using WheelOfFortune.Utils.RankDeterminer;
using Zenject;

namespace WheelOfFortune.Gameplay.Exit
{
    public class ExitButton : BaseButton
    {
        private readonly IExitButtonState _interactable = new InteractableState();
        private readonly IExitButtonState _nonInteractable = new NonInteractableState();

        private IEventBus _eventBus;
        private RankDeterminer _rankDeterminer;

        private IExitButtonState _currentState;
        private Rank _currentRank;
        private bool _isSpinning;

        [Inject]
        public void Construct(IEventBus eventBus, RankDeterminer rankDeterminer)
        {
            _eventBus = eventBus;
            _rankDeterminer = rankDeterminer;
        }

        private void Start()
        {
            ChangeState(_interactable);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _eventBus.Subscribe<OnGameStartEvent>(OnGameStart);
            _eventBus.Subscribe<LevelChangedEvent>(OnLevelChanged);
            _eventBus.Subscribe<SpinButtonClickedEvent>(OnSpinButtonClicked);
            _eventBus.Subscribe<OnRewardEffectArrivedEvent>(OnRewardEffectArrived);
            _eventBus.Subscribe<OnCollectBombEvent>(OnCollectBomb);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _eventBus.UnSubscribe<OnGameStartEvent>(OnGameStart);
            _eventBus.UnSubscribe<LevelChangedEvent>(OnLevelChanged);
            _eventBus.UnSubscribe<SpinButtonClickedEvent>(OnSpinButtonClicked);
            _eventBus.UnSubscribe<OnRewardEffectArrivedEvent>(OnRewardEffectArrived);
            _eventBus.UnSubscribe<OnCollectBombEvent>(OnCollectBomb);
        }

        protected override void OnButtonClicked()
        {
            _eventBus.Publish(new OnExitButtonClickedEvent());
        }

        public void ApplyInteractable(bool value)
        {
            SetInteractable(value);
        }

        private void OnGameStart(OnGameStartEvent eventData)
        {
            _currentRank = _rankDeterminer.DetermineRank(eventData.StartLevel);
            EvaluateState();
        }

        private void OnLevelChanged(LevelChangedEvent eventData)
        {
            _currentRank = _rankDeterminer.DetermineRank(eventData.NewLevel);
            EvaluateState();
        }

        private void OnSpinButtonClicked(SpinButtonClickedEvent _)
        {
            _isSpinning = true;
            EvaluateState();
        }

        private void OnRewardEffectArrived(OnRewardEffectArrivedEvent _)
        {
            _isSpinning = false;
            EvaluateState();
        }

        private void OnCollectBomb(OnCollectBombEvent _)
        {
            _isSpinning = false;
            EvaluateState();
        }

        private void EvaluateState()
        {
            bool shouldLock = _currentRank == Rank.Bronze || _isSpinning;
            ChangeState(shouldLock ? _nonInteractable : _interactable);
        }

        private void ChangeState(IExitButtonState next)
        {
            if (_currentState == next)
                return;

            _currentState?.Exit(this);
            _currentState = next;
            _currentState.Enter(this);
        }
    }
}
