using UnityEngine;

namespace WheelOfFortune.Gameplay.LevelDisplayManager
{
    [CreateAssetMenu(fileName = "LevelDisplayConfig", menuName = "WheelOfFortune/LevelDisplayManager/LevelDisplayConfig")]
    public class LevelDisplayConfig : ScriptableObject
    {
        [SerializeField] private int _maxLevel;
        [SerializeField] private int _startLevel;

        public int MaxLevel => _maxLevel;
        public int StartLevel => _startLevel;
    }
}