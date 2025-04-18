using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BulletPool : MonoBehaviour
{

    public static BulletPool instance;

    public GameObject prefab;
    public int initialSize = 30;

    private Queue<GameObject> pool = new Queue<GameObject>();

    public IObjectPool<GameObject> Pool { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }


        for (int i = 0; i < initialSize; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.gameObject.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public GameObject Get()
    {
        GameObject obj;

        if (pool.Count > 0)
        {
            obj = pool.Dequeue();
        }
        else
        {
            obj = Instantiate(prefab);
        }

        obj.gameObject.SetActive(true);
        return obj;
    }

    public void ReturnToPool(GameObject obj)
    {
        obj.gameObject.SetActive(false);
        pool.Enqueue(obj);
    }
}
