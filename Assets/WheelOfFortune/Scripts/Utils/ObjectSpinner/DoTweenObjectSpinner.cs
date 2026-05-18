using DG.Tweening;
using UnityEngine;

namespace WheelOfFortune.Utils.ObjectSpinner
{
    public class DoTweenObjectSpinner : IObjectSpinner
    {
        private SpinnerConfig _config;

        public void Initialize(SpinnerConfig config)
        {
            _config = config;
        }

        public void SpinObject(Transform objectToSpin, int sliceCount, int targetSliceIndex, System.Action onComplete = null)
        {
            float sliceAngle = 360f / sliceCount;
            int randomSpins = Random.Range(_config.MinSpinCount, _config.MaxSpinCount);
            float targetAngle = (360f * randomSpins) + sliceAngle * targetSliceIndex;

            var tweener = objectToSpin
                .DORotate(new Vector3(0, 0, targetAngle * (int)_config.ScrollDirection), _config.SpinDuration, RotateMode.FastBeyond360)
                .SetEase(_config.SpinEase)
                .OnComplete(() => onComplete?.Invoke());
        }
    }
}