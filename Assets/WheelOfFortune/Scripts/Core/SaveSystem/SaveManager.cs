using UnityEngine;

namespace WheelOfFortune.Core.SaveSystem
{
    public class SaveManager : ISaveManager
    {
        public void Save<T>(string key, T data) where T : struct
        {
            string jsonData = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(key, jsonData);
            PlayerPrefs.Save();
        }

        public T Load<T>(string key, T defaultValue) where T : struct
        {
            if (!PlayerPrefs.HasKey(key))
            {
                return defaultValue;
            }

            string jsonData = PlayerPrefs.GetString(key);
            return JsonUtility.FromJson<T>(jsonData);
        }
    }
}