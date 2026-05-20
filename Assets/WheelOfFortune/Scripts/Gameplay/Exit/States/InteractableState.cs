namespace WheelOfFortune.Gameplay.Exit.States
{
    public class InteractableState : IExitButtonState
    {
        public void Enter(ExitButton button)
        {
            button.ApplyInteractable(true);
        }

        public void Exit(ExitButton button)
        {
        }
    }
}
