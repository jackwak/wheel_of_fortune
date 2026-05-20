using WheelOfFortune.Enums;

namespace WheelOfFortune.Gameplay.Wheel.States
{
    public interface IWheelRankState
    {
        Rank Rank { get; }
        void Enter(WheelRankDisplay display);
        void Exit(WheelRankDisplay display);
    }
}
