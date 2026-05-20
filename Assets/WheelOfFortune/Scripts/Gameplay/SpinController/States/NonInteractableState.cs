namespace WheelOfFortune.Gameplay.SpinController.States
{
    public class NonInteractableState : ISpinButtonState
    {
        public void Enter(SpinButton button)
        {
            button.ApplyInteractable(false);
        }

        public void Exit(SpinButton button)
        {
        }
    }
}
