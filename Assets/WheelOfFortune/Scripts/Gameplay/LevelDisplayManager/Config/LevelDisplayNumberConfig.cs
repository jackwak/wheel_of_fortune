using UnityEngine;

namespace WheelOfFortune.Gameplay.LevelDisplayManager.Config
{
    [CreateAssetMenu(fileName = "LevelDisplayNumberConfig", menuName = "WheelOfFortune/LevelDisplayManager/LevelDisplayNumberConfig")]
    public class LevelDisplayNumberConfig : ScriptableObject
    {
        [SerializeField] private float _numberSpacing = 30f;
        [SerializeField] private float _numberWidth = 50f;
        
        [SerializeField] private float _numberScrollDuration = 1f;

        public float NumberSpacing => _numberSpacing;
        public float NumberWidth => _numberWidth;
        public float StepSize => _numberSpacing + _numberWidth;

        public float ScrollDuration => _numberScrollDuration;
    }
}