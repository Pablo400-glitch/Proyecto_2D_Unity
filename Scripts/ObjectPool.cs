using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject prefab; // Prefab del objeto a reutilizar
    public int poolSize = 10; // Número inicial de objetos en el pool
    private Queue<GameObject> pool = new Queue<GameObject>();

    protected virtual void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab, transform); // Asignar el objeto como hijo del pool
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public virtual GameObject GetObject()
    {
        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            GameObject obj = Instantiate(prefab, transform); // Asignar el objeto como hijo del pool
            obj.SetActive(true);
            return obj;
        }
    }

    public virtual void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.SetParent(transform); // Asegurar que el objeto regrese como hijo del pool
        pool.Enqueue(obj);
    }
}
