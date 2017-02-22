using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour 
{
    Dictionary <int, Queue<GameObject>> poolDictionary = new Dictionary<int, Queue<GameObject>>();

    static PoolManager _instance;

    public static PoolManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PoolManager>();
            }
            return _instance;
        }
    }

    public void CreatePool(GameObject pooledObject, int poolAmount)
    {
        int poolKey = pooledObject.GetInstanceID();

        if (!poolDictionary.ContainsKey(poolKey))
        {
            poolDictionary.Add(poolKey, new Queue<GameObject>());

            for (int i = 0; i < poolAmount; i++)
            {
                GameObject newObject = Instantiate (pooledObject) as GameObject;
                // Not visible in the scene yet
                newObject.SetActive (false);
                // Add to pool by passing poolkey
                poolDictionary[poolKey].Enqueue(newObject);
            }
        }
    }

    // Method for reusing
    public void ReuseObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        int poolKey = prefab.GetInstanceID();

        // Make sure to quick check contains poolKey
        if (poolDictionary.ContainsKey(poolKey))
        {

            GameObject objectToReuse = poolDictionary[poolKey].Dequeue();
            poolDictionary[poolKey].Enqueue (objectToReuse);

            objectToReuse.SetActive (true);
            objectToReuse.transform.position = position;
            objectToReuse.transform.rotation = rotation;
        }
    }
}
