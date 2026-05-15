using TMPro;
using UnityEngine;
using WheelOfFortune.Config;
using WheelOfFortune.Utils.RankDeterminer;
using Zenject;

namespace WheelOfFortune.Gameplay.LevelDisplayManager
{
    public class LevelDisplayNumberBehaviour : MonoBehaviour
    {
        [Header("Data")]
        private LevelDisplayNumberData _numberData;

        [Header("Dependencies")]
        private RankDeterminer _rankDeterminer;
        private RankColorPalette _rankPalette;
        
        [Header("References")]
        [SerializeField] private TextMeshProUGUI _levelNumberText;

        [Inject]
        public void Construct(RankDeterminer rankDeterminer, RankColorPalette rankPalette)
        {
            _rankDeterminer = rankDeterminer;
            _rankPalette = rankPalette;
        }

        public void Initialize(LevelDisplayNumberData numberData)
        {
            _numberData = numberData;
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            _levelNumberText.text = _numberData.LevelNumber.ToString();
            _levelNumberText.color =  _rankPalette.GetColor(_rankDeterminer.DetermineRank(_numberData.LevelNumber));
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
}