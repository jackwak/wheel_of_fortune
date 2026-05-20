namespace WheelOfFortune.Gameplay.Exit.States
{
    public interface IExitButtonState
    {
        void Enter(ExitButton button);
        void Exit(ExitButton button);
    }
}
