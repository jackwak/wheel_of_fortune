using DG.Tweening;
using UnityEngine;

namespace WheelOfFortune.Utils.ObjectSpinner
{
    [CreateAssetMenu(fileName = "SpinnerConfig", menuName = "WheelOfFortune/Config/SpinnerConfig")]
    public class SpinnerConfig : ScriptableObject
    {
        [Header("Spin Settings")]
        public int minSpinCount = 3;
        public int maxSpinCount = 7;
        public float spinDuration = 3f;
        public Ease spinEase = Ease.InExpo;
        public SpinScrollDirection scrollDirection = SpinScrollDirection.Clockwise;
    }

    public enum SpinScrollDirection
    {
        Clockwise = -1,
        CounterClockwise = 1
    }
}