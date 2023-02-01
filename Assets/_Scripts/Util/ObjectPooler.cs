using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Util
{
    public class ObjectPooler : MonoBehaviour
    {
        [System.Serializable]
        public class Pool
        {
            public string tag;
            public GameObject gameObject;
            public int size;
        }

        #region "Singleton"

        public static ObjectPooler Instance;

        private void Awake()
        {
            Instance = this;
            SetPool();
        }

        #endregion

        private Dictionary<string, Queue<GameObject>> _poolDictionary = new Dictionary<string, Queue<GameObject>>();
        public List<Pool> pools;

        private void SetPool()
        {
            var inActiveGemsParent = new GameObject("InActiveGemsParent");

            foreach (var pool in pools)
            {
                var objectPool = new Queue<GameObject>();

                for (int i = 0; i < pool.size; i++)
                {
                    var obj = Instantiate(pool.gameObject, inActiveGemsParent.transform);
                    obj.SetActive(false);
                    objectPool.Enqueue(obj);
                }

                _poolDictionary.Add(pool.tag, objectPool);
            }
        }

        public GameObject SpawnFromPool(string prefabTag, Vector3 position, Quaternion rotation, Transform parent)
        {
            if (!_poolDictionary.ContainsKey(prefabTag)) return null;

            var objectToSpawn = _poolDictionary[prefabTag].Dequeue();

            objectToSpawn.SetActive(transform);
            objectToSpawn.transform.position = position;
            objectToSpawn.transform.rotation = rotation;
            objectToSpawn.transform.SetParent(parent);
            _poolDictionary[prefabTag].Enqueue(objectToSpawn);

            return objectToSpawn;
        }
    }
}