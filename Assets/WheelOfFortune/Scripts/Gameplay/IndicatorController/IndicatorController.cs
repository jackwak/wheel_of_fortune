using DG.Tweening;
using UnityEngine;

public class IndicatorController : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private IndicatorControllerConfig _config;
    [SerializeField] private LayerMask _triggerLayerMask;
    [SerializeField] private Transform _objectToRotate;

    private Tween _rotationTween;
    private Vector3 _initialRotation;

    private void Awake()
    {
        _initialRotation = _objectToRotate.localEulerAngles;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & _triggerLayerMask) == 0)
            return;

        RotateIndicator();
    }

    private void RotateIndicator()
    {
        _rotationTween?.Kill();

        _rotationTween = DOTween.Sequence().Append(_objectToRotate.DOLocalRotate(new Vector3(0, 0, _config.RotationAngle * _config.SpinDirectionMultiplier),_config.RotationDuration
            ).SetEase(_config.RotationEase)).Append(_objectToRotate.DOLocalRotate(_initialRotation,_config.RotationDuration
            ).SetEase(Ease.OutQuad))
            .OnComplete(() =>
            {
                _rotationTween = null;
            });
    }
}
