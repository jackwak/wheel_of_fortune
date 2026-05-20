namespace WheelOfFortune.Core.SaveSystem
{
    public interface ISaveManager
    {
        void Save<T>(string key, T data) where T : struct;
        T Load<T>(string key, T defaultValue) where T : struct;
    }
}