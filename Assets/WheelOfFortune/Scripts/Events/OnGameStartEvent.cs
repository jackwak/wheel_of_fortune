namespace WheelOfFortune.Events
{
    public struct OnGameStartEvent
    {
        public int StartLevel { get; }

        public OnGameStartEvent(int startLevel)
        {
            StartLevel = startLevel;
        }
    }
}