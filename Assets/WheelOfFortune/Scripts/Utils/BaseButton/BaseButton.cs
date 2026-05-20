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

        protected virtual void OnEnable()
        {
            _button.onClick.AddListener(OnButtonClicked);
        }

        protected virtual void OnDisable()
        {
            _button.onClick.RemoveListener(OnButtonClicked);
        }

        protected void SetInteractable(bool value)
        {
            _button.interactable = value;
        }

        protected abstract void OnButtonClicked();
    }
}