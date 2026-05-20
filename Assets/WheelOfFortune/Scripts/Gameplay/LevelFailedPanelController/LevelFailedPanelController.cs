using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WheelOfFortune.Core.EventSystem;
using WheelOfFortune.Gameplay.LevelFlow.Events;
using WheelOfFortune.Gameplay.Revive;
using WheelOfFortune.Gameplay.Revive.Events;
using Zenject;

namespace WheelOfFortune.Gameplay.LevelFailedPanelController
{
    public class LevelFailedPanelController : MonoBehaviour
    {
        [SerializeField] private GameObject _panel;
        [SerializeField] private CanvasGroup _buttonCanvasGroup;
        [SerializeField] private Button _reviveButton;
        [SerializeField] private Button _giveUpButton;
        [SerializeField] private TextMeshProUGUI _reviveCostText;

        [SerializeField] private float _fadeDuration = 0.5f;
        [SerializeField] private float _delayBeforeFade = .5f;

        private IEventBus _eventBus;
        private ReviveService _reviveService;

        [Inject]
        public void Construct(IEventBus eventBus, ReviveService reviveService)
        {
            _eventBus = eventBus;
            _reviveService = reviveService;
        }

        void OnEnable()
        {
            _reviveButton.onClick.AddListener(OnReviveButtonClicked);
            _giveUpButton.onClick.AddListener(OnGiveUpButtonClicked);

            _eventBus.Subscribe<OnLevelFailedEvent>(OnLevelFailed);
            _eventBus.Subscribe<OnPlayerRevivedEvent>(OnPlayerRevived);
        }

        void OnDisable()
        {
            _reviveButton.onClick.RemoveListener(OnReviveButtonClicked);
            _giveUpButton.onClick.RemoveListener(OnGiveUpButtonClicked);

            _eventBus.UnSubscribe<OnLevelFailedEvent>(OnLevelFailed);
            _eventBus.UnSubscribe<OnPlayerRevivedEvent>(OnPlayerRevived);
        }

        private void OnReviveButtonClicked()
        {
            _eventBus.Publish(new OnReviveRequestedEvent());
        }

        private void OnGiveUpButtonClicked()
        {
            _eventBus.Publish(new OnGiveUpRequestedEvent());
        }

        private void OnLevelFailed(OnLevelFailedEvent _)
        {
            _panel.SetActive(true);
            _reviveCostText.text = _reviveService.GetCurrentReviveCost().ToString();
            _buttonCanvasGroup.DOFade(1, _fadeDuration).From(0).SetDelay(_delayBeforeFade);
        }

        private void OnPlayerRevived(OnPlayerRevivedEvent _)
        {
            _panel.SetActive(false);
        }
    }
}
