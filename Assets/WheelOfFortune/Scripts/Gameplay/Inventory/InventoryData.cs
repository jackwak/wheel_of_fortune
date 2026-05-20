using System;
using System.Collections.Generic;

namespace WheelOfFortune.Gameplay.Inventory
{
    [Serializable]
    public struct InventoryData
    {
        public List<ItemData> Items;

        [NonSerialized]
        public List<ItemData> PendingItems;
    }

    [Serializable]
    public struct ItemData
    {
        public string Name;
        public int Count;

        public ItemData(string name, int count)
        {
            Name = name;
            Count = count;
        }
    }
}