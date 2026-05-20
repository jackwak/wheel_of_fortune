using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using WheelOfFortune.Core.EventSystem;
using WheelOfFortune.Gameplay.Exit.Events;
using Zenject;

namespace WheelOfFortune.Gameplay.Exit
{
    public class ExitPanelController : MonoBehaviour
    {
        [SerializeField] private GameObject _exitPanel;
        [SerializeField] private CanvasGroup _buttonsCanvasGroup;
        [SerializeField] private Button _collectButton;
        [SerializeField] private Button _goBackButton;

        [SerializeField] private float _fadeDuration = 0.5f;
        [SerializeField] private float _delayBeforeFade = .5f;

        private IEventBus _eventBus;

        [Inject]
        public void Construct(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        private void OnEnable()
        {
            _eventBus.Subscribe<OnExitButtonClickedEvent>(OnExitButtonClicked);

            _collectButton.onClick.AddListener(OnCollectButtonClicked);
            _goBackButton.onClick.AddListener(OnGoBackButtonClicked);
        }

        private void OnDisable()
        {
            _eventBus.UnSubscribe<OnExitButtonClickedEvent>(OnExitButtonClicked);

            _collectButton.onClick.RemoveListener(OnCollectButtonClicked);
            _goBackButton.onClick.RemoveListener(OnGoBackButtonClicked);
        }

        private void OnExitButtonClicked(OnExitButtonClickedEvent e)
        {
            _exitPanel.SetActive(true);
            _buttonsCanvasGroup.DOFade(1, _fadeDuration).From(0).SetDelay(_delayBeforeFade);
        }

        private void OnCollectButtonClicked()
        {
            _eventBus.Publish(new OnExitGameEvent());
        }

        private void OnGoBackButtonClicked()
        {
            _exitPanel.SetActive(false);
        }
    }
}
