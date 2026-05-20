using DG.Tweening;
using UnityEngine;
using WheelOfFortune.Utils.ObjectSpinner;

namespace WheelOfFortune.Gameplay.IndicatorController
{
    [CreateAssetMenu(fileName = "IndicatorControllerConfig", menuName = "WheelOfFortune/Gameplay/IndicatorController/IndicatorControllerConfig")]
    public class IndicatorControllerConfig : ScriptableObject
    {
        [SerializeField] private float _rotationAngle = 60f;
        [SerializeField] private float _rotationDuration = 0.5f;
        [SerializeField] private Ease _rotationEase = Ease.OutQuad;
        [SerializeField] private SpinnerConfig _spinnerConfig;

        public float RotationAngle => _rotationAngle;
        public float RotationDuration => _rotationDuration;
        public Ease RotationEase => _rotationEase;
        public int SpinDirectionMultiplier => (int)_spinnerConfig.ScrollDirection * -1;
    }
}
