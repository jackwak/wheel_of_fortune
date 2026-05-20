using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace WheelOfFortune.Core.ObjectPool
{
    public class PoolManager : MonoBehaviour
    {
        [SerializeField] private List<PoolEntry> _entries;

        private readonly Dictionary<GameObject, ObjectPool> _pools = new();

        [Inject]
        public void Initialize()
        {
            foreach (var entry in _entries)
            {
                Transform poolParent = CreatePoolRoot(entry.Prefab);
                _pools[entry.Prefab] = new ObjectPool(entry.Prefab, entry.InitialCount, poolParent);
            }
        }

        private Transform CreatePoolRoot(GameObject prefab)
        {
            GameObject root = new GameObject($"Pool_{prefab.name}");
            if (prefab.GetComponent<RectTransform>() != null)
                root.AddComponent<RectTransform>();

            Transform t = root.transform;
            t.SetParent(transform, false);
            return t;
        }

        public GameObject Get(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            if (!_pools.TryGetValue(prefab, out var pool))
            {
                Debug.LogWarning($"Pool couldnt be found for '{prefab.name}'.");
                pool = CreatePool(prefab, 10);
            }

            return pool.Get(position, rotation);
        }

        public GameObject Get(GameObject prefab, Transform parent, Vector3 position, Quaternion rotation, bool worldPositionStays = true)
        {
            if (!_pools.TryGetValue(prefab, out var pool))
            {
                Debug.LogWarning($"Pool couldnt be found for '{prefab.name}'.");
                pool = CreatePool(prefab, 10);
            }

            return pool.Get(parent, position, rotation, worldPositionStays);
        }

        public GameObject Get(GameObject prefab, Transform parent, bool worldPositionStays = true)
        {
            return Get(prefab, parent, Vector3.zero, Quaternion.identity, worldPositionStays);
        }

        public void SetParent(GameObject prefab, Transform parent, bool worldPositionStays = false)
        {
            if (parent == null)
            {
                Debug.LogWarning($"SetParent called with null parent for '{prefab.name}'.");
                return;
            }

            if (!_pools.TryGetValue(prefab, out var pool))
            {
                Debug.LogWarning($"Pool couldnt be found for '{prefab.name}'.");
                return;
            }

            pool.SetParent(parent, worldPositionStays);
        }

        public void Release(GameObject prefab, GameObject obj)
        {
            if (_pools.TryGetValue(prefab, out var pool))
                pool.Release(obj);
            else
                Debug.LogWarning($"Pool couldnt be found for '{prefab.name}'.");
        }

        private ObjectPool CreatePool(GameObject prefab, int initialCount)
        {
            Transform poolParent = CreatePoolRoot(prefab);
            var pool = new ObjectPool(prefab, initialCount, poolParent);
            _pools[prefab] = pool;
            return pool;
        }
    }
}