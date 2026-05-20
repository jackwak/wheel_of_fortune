using WheelOfFortune.Enums;

namespace WheelOfFortune.Gameplay.Wheel.States
{
    public class GoldWheelRankState : IWheelRankState
    {
        public Rank Rank => Rank.Gold;

        public void Enter(WheelRankDisplay display)
        {
            display.ApplyVisuals(Rank);
        }

        public void Exit(WheelRankDisplay display)
        {
        }
    }
}
