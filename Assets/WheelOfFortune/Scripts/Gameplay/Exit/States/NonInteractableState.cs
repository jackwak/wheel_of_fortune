namespace WheelOfFortune.Gameplay.Exit.States
{
    public class NonInteractableState : IExitButtonState
    {
        public void Enter(ExitButton button)
        {
            button.ApplyInteractable(false);
        }

        public void Exit(ExitButton button)
        {
        }
    }
}
