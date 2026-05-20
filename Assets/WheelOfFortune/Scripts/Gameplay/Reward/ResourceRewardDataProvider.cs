using System.Collections.Generic;
using UnityEngine;
using WheelOfFortune.Enums;

namespace WheelOfFortune.Gameplay.Reward
{
    public class ResourcesRewardDataProvider : IRewardDataProvider
    {
        private readonly List<RewardCatalogEntry> _allCatalogEntries;
        private readonly Dictionary<Rank, List<RewardCatalogEntry>> _catalogEntriesByRank;

        public ResourcesRewardDataProvider()
        {
            _allCatalogEntries = new List<RewardCatalogEntry>(Resources.LoadAll<RewardCatalogEntry>("Rewards"));

            _catalogEntriesByRank = new Dictionary<Rank, List<RewardCatalogEntry>>();

            foreach (RewardCatalogEntry catalogEntry in _allCatalogEntries)
            {
                Rank rank = catalogEntry.Rank;

                if (!_catalogEntriesByRank.ContainsKey(rank))
                {
                    _catalogEntriesByRank[rank] = new List<RewardCatalogEntry>();
                }

                _catalogEntriesByRank[rank].Add(catalogEntry);
            }
        }

        public RewardData[] GetRewardDataByRank(Rank rank)
        {
            if (!_catalogEntriesByRank.TryGetValue(rank, out List<RewardCatalogEntry> entries))
            {
                return new RewardData[0];
            }

            RewardData[] result = RewardDataByRewardCatalogEntry(entries.ToArray());

            return result;
        }

        public RewardData[] GetRandomRewardDataByRank(Rank rank, int count)
        {
            if (!_catalogEntriesByRank.TryGetValue(rank, out List<RewardCatalogEntry> entries))
            {
                return new RewardData[0];
            }

            if (count > entries.Count)
            {
                Debug.LogWarning($"Requested reward count {count} is greater than available entries {entries.Count} for rank {rank}. Returning all available entries.");
                return new RewardData[0];
            }

            List<RewardCatalogEntry> shuffledEntries = new List<RewardCatalogEntry>(entries);

            for (int i = 0; i < shuffledEntries.Count; i++)
            {
                int randomIndex = Random.Range(i, shuffledEntries.Count);

                (shuffledEntries[i], shuffledEntries[randomIndex]) = (shuffledEntries[randomIndex], shuffledEntries[i]);
            }

            RewardCatalogEntry[] result = new RewardCatalogEntry[count];

            for (int i = 0; i < count; i++)
            {
                result[i] = shuffledEntries[i];
            }

            RewardData[] rewardData = RewardDataByRewardCatalogEntry(result);

            return rewardData;
        }

        private RewardData[] RewardDataByRewardCatalogEntry(RewardCatalogEntry[] entries)
        {
            RewardData[] result = new RewardData[entries.Length];
            for (int i = 0; i < entries.Length; i++)
            {
                RewardCatalogEntry entry = entries[i];
                result[i] = new RewardData(entry.Definition, Random.Range(entry.CountRange.x, entry.CountRange.y + 1));
            }

            return result;
        }
    }
}
