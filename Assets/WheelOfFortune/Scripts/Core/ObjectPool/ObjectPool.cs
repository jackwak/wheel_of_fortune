using System.Collections.Generic;
using UnityEngine;

namespace WheelOfFortune.Core.ObjectPool
{
    public class ObjectPool
    {
        private readonly GameObject _prefab;
        private readonly Transform _parent;
        private readonly Queue<GameObject> _available = new();

        public ObjectPool(GameObject prefab, int initialCount, Transform parent)
        {
            _prefab = prefab;
            _parent = parent;

            for (int i = 0; i < initialCount; i++)
            {
                GameObject obj = CreateNew();
                obj.SetActive(false);
                _available.Enqueue(obj);
            }
        }

        public GameObject Get(Vector3 position, Quaternion rotation)
        {
            GameObject obj = _available.Count > 0 ? _available.Dequeue() : CreateNew();

            obj.transform.SetPositionAndRotation(position, rotation);
            obj.SetActive(true);
            return obj;
        }

        public GameObject Get(Transform parent, Vector3 position, Quaternion rotation, bool worldPositionStays = true)
        {
            GameObject obj = _available.Count > 0 ? _available.Dequeue() : CreateNew();

            if (parent != null)
                obj.transform.SetParent(parent, worldPositionStays);

            obj.transform.SetPositionAndRotation(position, rotation);
            obj.SetActive(true);
            return obj;
        }

        public void SetParent(Transform parent, bool worldPositionStays = false)
        {
            _parent.SetParent(parent, worldPositionStays);
        }

        public void Release(GameObject obj)
        {
            obj.SetActive(false);

            // Ensure pooled objects always return under the pool root.
            obj.transform.SetParent(_parent, false);

            _available.Enqueue(obj);
        }

        private GameObject CreateNew()
        {
            return Object.Instantiate(_prefab, _parent, false);
        }
    }
}