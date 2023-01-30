using System.Collections.Generic;
using UnityEngine;

namespace _Scripts
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
        private Transform _activeObjectParent;

        private void SetPool()
        {
            _activeObjectParent = new GameObject("ActiveGemParent").transform;
            GameObject inActiveCubesParent = new GameObject("InActiveGemsParent");

            foreach (Pool pool in pools)
            {
                Queue<GameObject> objectPool = new Queue<GameObject>();

                for (int i = 0; i < pool.size; i++)
                {
                    GameObject obj = Instantiate(pool.gameObject, inActiveCubesParent.transform);
                    obj.SetActive(false);
                    objectPool.Enqueue(obj);
                }

                _poolDictionary.Add(pool.tag, objectPool);
            }
        }

        public GameObject SpawnGemFromPool(string tag, Vector3 position, Quaternion rotation)
        {
            if (!_poolDictionary.ContainsKey(tag)) return null;

            GameObject objectToSpawn = _poolDictionary[tag].Dequeue();

            objectToSpawn.SetActive(transform);
            objectToSpawn.transform.position = position;
            objectToSpawn.transform.rotation = rotation;
            objectToSpawn.transform.SetParent(_activeObjectParent);
            _poolDictionary[tag].Enqueue(objectToSpawn);
            

            return objectToSpawn;
        }
    }
}