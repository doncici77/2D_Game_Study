using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance;

    [System.Serializable]
    public class PoolItem
    {
        public string key; // ��: "Bullet", "Missile"
        public GameObject prefab;
        public int initialSize = 10;
    }

    public List<PoolItem> itemsToPool;

    private Dictionary<string, Queue<GameObject>> poolDict = new Dictionary<string, Queue<GameObject>>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        // �� Ǯ �ʱ�ȭ
        foreach (var item in itemsToPool)
        {
            Queue<GameObject> newQueue = new Queue<GameObject>();

            for (int i = 0; i < item.initialSize; i++)
            {
                GameObject obj = Instantiate(item.prefab);
                obj.SetActive(false);
                newQueue.Enqueue(obj);
            }

            poolDict.Add(item.key, newQueue);
        }
    }

    public GameObject Get(string key)
    {
        if (!poolDict.ContainsKey(key))
        {
            Debug.LogWarning($"Ǯ�� {key}�� �����ϴ�.");
            return null;
        }

        Queue<GameObject> queue = poolDict[key];

        GameObject obj;
        if (queue.Count > 0)
        {
            obj = queue.Dequeue();
        }
        else
        {
            // �߰� �ν��Ͻ� ����
            var item = itemsToPool.Find(x => x.key == key);
            obj = Instantiate(item.prefab);
        }

        obj.SetActive(true);
        return obj;
    }

    public void ReturnToPool(string key, GameObject obj)
    {
        obj.SetActive(false);

        if (poolDict.ContainsKey(key))
        {
            poolDict[key].Enqueue(obj);
        }
        else
        {
            Destroy(obj); // ��ϵ��� ���� Ǯ�̸� �ı�
        }
    }
}
