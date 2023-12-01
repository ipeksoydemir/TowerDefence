namespace Tools.ObjectPooling
{
    using System.Collections.Generic;
    using UnityEngine;

    public class ObjectPool
    {
        private HashSet<GameObject> _activeObjects = new HashSet<GameObject>();
        private Queue<GameObject> _pool = new Queue<GameObject>();
        private GameObject _objectPrefab;
        private Transform _parentTransform;

        public ObjectPool(GameObject prefab, int initialSize, Transform parentTransform)
        {
            _objectPrefab = prefab;
            _parentTransform = parentTransform;
            for (int i = 0; i < initialSize; i++)
            {
                AddObject();
            }
        }

        private void AddObject()
        {
            var newObject = GameObject.Instantiate(_objectPrefab, _parentTransform);
            newObject.SetActive(false);
            _pool.Enqueue(newObject);
        }

        public GameObject GetObject()
        {
            GameObject obj;
            if (_pool.Count > 0)
            {
                obj = _pool.Dequeue();
            }
            else
            {
                obj = GameObject.Instantiate(_objectPrefab, _parentTransform);
            }

            obj.SetActive(true);
            _activeObjects.Add(obj);
            return obj;
        }

        public void ReturnObject(GameObject returnedObject)
        {
            if (_activeObjects.Contains(returnedObject))
            {
                returnedObject.SetActive(false);
                _pool.Enqueue(returnedObject);
                _activeObjects.Remove(returnedObject);
            }
        }
    }

}