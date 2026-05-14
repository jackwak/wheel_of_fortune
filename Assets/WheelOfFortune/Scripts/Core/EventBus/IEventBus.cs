namespace WheelOfFortune.Core.EventBus
{
    public interface IEventBus
    {
        void Subscribe<T>(System.Action<T> callback) where T : struct;
        void Unsubscribe<T>(System.Action<T> callback) where T : struct;
        void Publish<T>(T eventData) where T : struct;
    }
}
