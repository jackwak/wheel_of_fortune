namespace WheelOfFortune.Events
{
    public readonly struct WheelSliceSelectedEvent
    {
        public int SliceIndex { get; }

        public WheelSliceSelectedEvent(int sliceIndex)
        {
            SliceIndex = sliceIndex;
        }
    }
}
