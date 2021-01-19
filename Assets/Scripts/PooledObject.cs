using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PooledObject 
{
    [SerializeField] private GameObject objectToPool;
    [SerializeField] private int amountToPool;

    private List<GameObject> pooledObjects = new List<GameObject>();

    public void InitialisePool()
    {
        GameObject thePool = new GameObject();
        thePool.transform.position = Vector3.zero;
        thePool.name = "Pooled " + objectToPool.name + "s";

        GameObject tmp;
        for (int i = 0; i < amountToPool; i++)
        {
            tmp = GameObject.Instantiate(objectToPool);
            tmp.gameObject.SetActive(false);
            tmp.transform.SetParent(thePool.transform);
            pooledObjects.Add(tmp);
        }
    }

    public GameObject GetFromPool()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].gameObject.activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        return null;
    }
}

