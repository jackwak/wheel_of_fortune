using TMPro;
using UnityEngine;

public class LevelDisplayNumberBehaviour : MonoBehaviour
{
    [Header("Data")]
    private LevelDisplayNumberData _numberData;

    [Header("References")]
    [SerializeField] private TextMeshProUGUI _levelNumberText;

    public void Initialize(LevelDisplayNumberData numberData)
    {
        _numberData = numberData;
        UpdateDisplay();
    }
    
    private void UpdateDisplay()
    {
        _levelNumberText.text = _numberData.LevelNumber.ToString();
    }
}

public struct LevelDisplayNumberData
{
    public int LevelNumber { get; }

    public LevelDisplayNumberData(int levelNumber)
    {
        LevelNumber = levelNumber;
    }
}
