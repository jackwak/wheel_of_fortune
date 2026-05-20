using TMPro;
using UnityEngine;

namespace WheelOfFortune.Gameplay.CurrencyDisplayController
{
    public class CurrencyDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _currencyText;

        public void UpdateCurrency(int newAmount)
        {
            _currencyText.text = newAmount.ToString();
        }
    }
}