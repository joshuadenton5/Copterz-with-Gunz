using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager _instance;
    [SerializeField] List<PooledObject> pooled = new List<PooledObject>();

    private void Awake()
    {
        _instance = this;
        foreach (PooledObject p in pooled)
            p.InitialisePool();
    }  

    public GameObject GetFromPool(int index)
    {
        return pooled[index].GetFromPool();
    }    
}
