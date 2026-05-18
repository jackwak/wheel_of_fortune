namespace WheelOfFortune.Events
{
    public struct LevelChangedEvent
    {
        public int NewLevel { get; }

        public LevelChangedEvent(int newLevel)
        {
            NewLevel = newLevel;
        }
    }
}
