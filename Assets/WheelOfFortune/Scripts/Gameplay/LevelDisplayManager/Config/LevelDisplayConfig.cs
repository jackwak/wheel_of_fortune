using UnityEngine;

namespace WheelOfFortune.Gameplay.LevelDisplayManager
{
    [CreateAssetMenu(fileName = "LevelDisplayConfig", menuName = "ScriptableObjects/LevelDisplayManager/LevelDisplayConfig")]
    public class LevelDisplayConfig : ScriptableObject
    {
        [SerializeField] private int _maxLevel;

        public int MaxLevel => _maxLevel;
    }
}