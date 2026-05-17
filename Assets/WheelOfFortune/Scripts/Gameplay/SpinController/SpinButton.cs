using WheelOfFortune.Core.EventBus;
using WheelOfFortune.Gameplay.SpinController.Events;
using WheelOfFortune.Utils.BaseButton;
using Zenject;

namespace WheelOfFortune.Gameplay.SpinController
{
    public class SpinButton : BaseButton
    {
        private IEventBus _eventBus;

        [Inject]
        private void Construct(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        protected override void OnButtonClicked()
        {
            _eventBus.Publish(new SpinButtonClickedEvent());
        }
    }
}