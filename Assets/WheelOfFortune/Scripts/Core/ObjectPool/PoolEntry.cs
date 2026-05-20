using UnityEngine;

namespace WheelOfFortune.Core.ObjectPool
{
    [System.Serializable]
    public class PoolEntry
    {
        public GameObject Prefab;
        public int InitialCount;
    }
}