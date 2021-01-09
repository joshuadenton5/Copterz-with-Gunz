using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager SharedInstance;
    [SerializeField] List<PooledObject> pooled = new List<PooledObject>();

    private void Awake()
    {
        SharedInstance = this;
        foreach (PooledObject p in pooled)
            p.InitialisePool();
    }  

    public GameObject GetFromPool(int index)
    {
        return pooled[index].GetFromPool();
    }    
}
