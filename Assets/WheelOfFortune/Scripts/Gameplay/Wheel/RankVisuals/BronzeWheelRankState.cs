using WheelOfFortune.Enums;

namespace WheelOfFortune.Gameplay.Wheel.States
{
    public class BronzeWheelRankState : IWheelRankState
    {
        public Rank Rank => Rank.Bronze;

        public void Enter(WheelRankDisplay display)
        {
            display.ApplyVisuals(Rank);
        }

        public void Exit(WheelRankDisplay display)
        {
        }
    }
}
