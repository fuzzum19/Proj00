using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public class LevelGeneration
{
	public string name;
	public GameObject[] pooledObjects;
	public LevelType vLevelType;
}

public enum LevelType
{
	EasyLevels01,
	EasyLevels02,
	EasyLevels03,
}
*/

public class ObjectPooler : RexJumpElement
{
	public GameObject pooledObject;
	public GameObject parentofPooler;
	public int pooledAmount;

	List<GameObject> objectList;

	void Start()
    {
        objectList = new List<GameObject>();

		for (int i = 0; i < pooledAmount; i++)
		{
			GameObject obj = (GameObject)Instantiate(pooledObject);
			obj.transform.parent = parentofPooler.transform;
			obj.SetActive (false);
			objectList.Add(obj);
		}

	}

	public GameObject GetPooledObject()
    {

        for (int i = 0; i < objectList.Count; i++)
		{
			if (!objectList [i].activeInHierarchy)
			{
				return objectList[i];
			}
		}


		GameObject obj = (GameObject)Instantiate(pooledObject);
		obj.transform.parent = parentofPooler.transform;
		obj.SetActive (false);
		objectList.Add(obj);
		return obj;

	}
}
