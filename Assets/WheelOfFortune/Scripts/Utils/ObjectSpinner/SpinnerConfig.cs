using DG.Tweening;
using UnityEngine;

namespace WheelOfFortune.Utils.ObjectSpinner
{
    [CreateAssetMenu(fileName = "SpinnerConfig", menuName = "WheelOfFortune/Config/SpinnerConfig")]
    public class SpinnerConfig : ScriptableObject
    {
        [Header("Spin Settings")]
        [SerializeField] private int _minSpinCount = 3;
        [SerializeField] private int _maxSpinCount = 7;
        [SerializeField] private float _spinDuration = 3f;
        [SerializeField] private Ease _spinEase = Ease.InExpo;
        [SerializeField] private SpinScrollDirection _scrollDirection = SpinScrollDirection.Clockwise;
        
        public int MinSpinCount => _minSpinCount;
        public int MaxSpinCount => _maxSpinCount;
        public float SpinDuration => _spinDuration;
        public Ease SpinEase => _spinEase;
        public SpinScrollDirection ScrollDirection => _scrollDirection;
    }

    public enum SpinScrollDirection
    {
        Clockwise = -1,
        CounterClockwise = 1
    }
}