namespace WheelOfFortune.Gameplay.SpinController.States
{
    public class InteractableState : ISpinButtonState
    {
        public void Enter(SpinButton button)
        {
            button.ApplyInteractable(true);
        }

        public void Exit(SpinButton button)
        {
        }
    }
}
