using UnityEngine;

namespace WheelOfFortune.Utils.ObjectSpinner
{
    public interface IObjectSpinner
    {
        void Initialize(SpinnerConfig config);
        void SpinObject(Transform objectToSpin, int sliceCount, int targetSliceIndex, System.Action onComplete = null);
    }
}