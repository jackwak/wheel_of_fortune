namespace WheelOfFortune.Events
{
    public struct LevelChangedEventData
    {
        public int NewLevel { get; }

        public LevelChangedEventData(int newLevel)
        {
            NewLevel = newLevel;
        }
    }
}
