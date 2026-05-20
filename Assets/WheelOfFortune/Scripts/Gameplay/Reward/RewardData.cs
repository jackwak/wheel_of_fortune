namespace WheelOfFortune.Gameplay.Reward
{
    public class RewardData
    {
        public RewardDefinition Definition;
        public int Count;

        public RewardData(RewardDefinition definition, int count)
        {
            Definition = definition;
            Count = count;
        }
    }
}
