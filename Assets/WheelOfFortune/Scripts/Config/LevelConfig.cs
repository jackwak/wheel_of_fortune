using UnityEngine;

namespace WheelOfFortune.Config
{
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "WheelOfFortune/Config/LevelConfig")]
    public class LevelConfig : ScriptableObject
    {
        [SerializeField] private int _startLevel = 1;

        public int StartLevel => _startLevel;
    }
}