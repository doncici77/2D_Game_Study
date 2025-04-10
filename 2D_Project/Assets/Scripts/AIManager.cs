using UnityEngine;

public enum MonsterType
{
    Mon1,
    Mon2,
    Mon3
}

public class AIManager : MonoBehaviour
{
    public GameObject[] enemyPrefab;
    public float spawnRangeX = 10.0f; // 스폰 범위
    public float spawnRangeY = 5.0f;
    public int enemyCount = 5;
    public Transform[] spawnPoints;
    private float enemySpeed = 1.0f;
    public MonsterType enemyType;

    void Start()
    {
        SpawnEnemies();
    }

    void SpawnEnemies()
    {
        for(int i = 0; i < enemyCount; i++)
        {
            if(spawnPoints.Length > 0)
            {
                int randomIndex = Random.Range(0, spawnPoints.Length);
                Vector2 spawnPosition = spawnPoints[randomIndex].position;
                MonsterSetState();
                Instantiate(enemyPrefab[(int)enemyType], spawnPosition, Quaternion.identity);
            }
            else
            {
                float randomX = Random.Range(-spawnRangeX, spawnRangeY);
                float randomY = Random.Range(-spawnRangeX, spawnRangeY);
                Vector2 randomPosition = new Vector2(randomX, randomY);
                MonsterSetState();
                Instantiate(enemyPrefab[(int)enemyType], randomPosition, Quaternion.identity);
            }
            Debug.Log("(int)enemyType : " + (int)enemyType);
        }
    }

    void MonsterSetState()
    {
        EnemyManager enemy = enemyPrefab[(int)enemyType].GetComponent<EnemyManager>();
        float minSpeed = 1.0f;
        float maxSpeed = 10.0f;

        if(enemyType == MonsterType.Mon1)
        {
            minSpeed = 1.0f;
            maxSpeed = 5f;
        }
        else if (enemyType == MonsterType.Mon2)
        {
            minSpeed = 1.0f;
            maxSpeed = 2f;
        }
        else if (enemyType == MonsterType.Mon3)
        {
            minSpeed = 1.0f;
            maxSpeed = 5f;
        }

        enemySpeed = Random.Range(minSpeed, maxSpeed);
        enemy.speed = enemySpeed;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(Vector2.zero, new Vector2(spawnRangeX * 2, spawnRangeY * 2));
        Gizmos.color = Color.blue;
        if (spawnPoints.Length > 0)
        {
            foreach (Transform spawnPoint in spawnPoints)
            {
                Gizmos.DrawWireSphere(spawnPoint.position, 0.5f);
            }
        }
    }
}
