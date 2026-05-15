using TMPro;
using UnityEngine;
using WheelOfFortune.Core.EventBus;
using Zenject;
using WheelOfFortune.Events;
using WheelOfFortune.Config;
using WheelOfFortune.Enums;

public class ZoneDisplayController : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private ZoneNumberTextData[] zoneNumberTexts;

    [Header("Dependencies")]
    [SerializeField] private LevelRankConfig _levelRankConfig;
    [SerializeField] private LevelConfig _levelConfig;

    private IEventBus _eventBus;

    [Inject]
    public void Construct(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    private void OnEnable()
    {
        _eventBus.Subscribe<LevelChangedEventData>(OnLevelChanged);
    }

    private void OnDisable()
    {
        _eventBus.UnSubscribe<LevelChangedEventData>(OnLevelChanged);
    }

    private void Start()
    {
        Initialize(_levelConfig.StartLevel);
    }

    public void Initialize(int currentLevel)
    {
        RefreshZoneNumbers(currentLevel);
    }

    private void OnLevelChanged(LevelChangedEventData eventData)
    {
        RefreshZoneNumbers(eventData.NewLevel);
    }

    private void RefreshZoneNumbers(int currentLevel)
    {
        foreach (ZoneNumberTextData zoneNumberText in zoneNumberTexts)
        {
            if (zoneNumberText.Text == null)
                continue;

            int nextLevel = GetNextLevelForRank(currentLevel, zoneNumberText.Rank);

            zoneNumberText.Text.text = nextLevel.ToString();
        }
    }

    private int GetNextLevelForRank(int currentLevel, Rank targetRank)
    {
        int nextLevel = currentLevel + 1;

        while (true)
        {
            Rank rank = DetermineRank(nextLevel);

            if (rank == targetRank)
                return nextLevel;

            nextLevel++;
        }
    }

    private Rank DetermineRank(int levelNumber)
    {
        if (levelNumber == 1)
            return Rank.Silver;

        Rank resultRank = Rank.Bronze;
        int highestMatchedInterval = 0;

        foreach (LevelRankData rankData in _levelRankConfig.RankDatas)
        {
            if (rankData.RankInterval <= 0)
            {
                Debug.LogWarning($"Invalid rank interval: {rankData.RankInterval}");
                continue;
            }

            if (levelNumber % rankData.RankInterval != 0)
                continue;

            if (rankData.RankInterval > highestMatchedInterval)
            {
                highestMatchedInterval = rankData.RankInterval;
                resultRank = rankData.Rank;
            }
        }

        return resultRank;
    }
}

[System.Serializable]
public struct ZoneNumberTextData
{
    public Rank Rank;
    public TextMeshProUGUI Text;
}