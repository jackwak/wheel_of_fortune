using UnityEngine;
using UnityEngine.UI;

namespace WheelOfFortune.Utils.BaseButton
{
    [RequireComponent(typeof(Button))]
    public abstract class BaseButton : MonoBehaviour
    {
        private Button _button;

        void Awake()
        {
            _button = GetComponent<Button>();
        }

        void OnEnable()
        {
            _button.onClick.AddListener(OnButtonClicked);
        }

        void OnDisable()
        {
            _button.onClick.RemoveListener(OnButtonClicked);
        }

        protected abstract void OnButtonClicked();
    }
}