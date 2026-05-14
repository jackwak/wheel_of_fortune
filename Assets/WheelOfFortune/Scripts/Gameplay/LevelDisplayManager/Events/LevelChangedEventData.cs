namespace WheelOfFortune.Gameplay.LevelDisplayManager
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
