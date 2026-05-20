using WheelOfFortune.Enums;

namespace WheelOfFortune.Gameplay.Wheel.States
{
    public class SilverWheelRankState : IWheelRankState
    {
        public Rank Rank => Rank.Silver;

        public void Enter(WheelRankDisplay display)
        {
            display.ApplyVisuals(Rank);
        }

        public void Exit(WheelRankDisplay display)
        {
        }
    }
}
