using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject prefab; // Prefab of the object to pool
    public int poolSize = 10; // Initial pool size
    private Queue<GameObject> pool = new Queue<GameObject>();

    protected virtual void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab, transform); // Asign the object as a child of the pool
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public virtual GameObject GetObject()
    {
        // Check if there are objects available in the pool
        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            GameObject obj = Instantiate(prefab, transform); // Asign the object as a child of the pool
            obj.SetActive(true);
            return obj;
        }
    }

    public virtual void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.SetParent(transform); // Here I Ensure the object is a child of the pool
        pool.Enqueue(obj);
    }
}
