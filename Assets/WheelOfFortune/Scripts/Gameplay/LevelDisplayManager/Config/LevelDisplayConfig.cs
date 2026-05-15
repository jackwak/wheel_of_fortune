using UnityEngine;

namespace WheelOfFortune.Gameplay.LevelDisplayManager
{
    [CreateAssetMenu(fileName = "LevelDisplayConfig", menuName = "WheelOfFortune/LevelDisplayManager/LevelDisplayConfig")]
    public class LevelDisplayConfig : ScriptableObject
    {
        [SerializeField] private int _extraVisibleItemBuffer = 2;
        [SerializeField] private int _startLevel = 1;

        public int ExtraVisibleItemBuffer => _extraVisibleItemBuffer;
        public int StartLevel => _startLevel;
    }
}