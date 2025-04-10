using UnityEngine;

public enum EMonsterType
{
    Mon1,
    Mon2,
    Mon3
}


public class AIManager : MonoBehaviour
{
    public GameObject monsterPrefab;
    public float spawnRangeX = 10.0f;
    public float spawnRangeY = 5.0f;
    public int enemyCount = 5;
    public Transform[] spawnPoints;
    private float monsterSpeed = 1.0f;
    private EMonsterType currentMonsterType = EMonsterType.Mon1;
    private float monsterHp = 1;
    private float monsterDamage = 1;

    void Start()
    {
        SpawnEnemies();
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            if (spawnPoints.Length > 0)
            {
                int randomIndex = Random.Range(0, spawnPoints.Length);
                Vector2 spawnPosition = spawnPoints[randomIndex].position;
                Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);
            }
            else
            {
                float randomX = Random.Range(-spawnRangeX, spawnRangeX);
                float randomY = Random.Range(-spawnRangeY, spawnRangeY);
                Vector2 randomPosition = new Vector2(randomX, randomY);
                Instantiate(monsterPrefab, randomPosition, Quaternion.identity);
            }
        }
    }


    void MonsterSetState()
    {
        EnemyManager monster = monsterPrefab.GetComponent<EnemyManager>();
        float minSpeed = 1f;
        float maxSpeed = 10f;
        float minHp = 1f;
        float maxHp = 10f;
        float minDamage = 1f;
        float maxDamage = 10f;

        if (currentMonsterType == EMonsterType.Mon1)
        {
            minSpeed = 1;
            maxSpeed = 5;
            minHp = 1;
            maxHp = 10;
            minDamage = 1;
            maxDamage = 10;
        }
        else if (currentMonsterType == EMonsterType.Mon2)
        {
            minSpeed = 0.5f;
            maxSpeed = 3.0f;
            minHp = 5;
            maxHp = 12;
            minDamage = 3;
            maxDamage = 12;
        }
        else if (currentMonsterType == EMonsterType.Mon3)
        {
            minSpeed = 3.0f;
            maxSpeed = 7.0f;
            minHp = 1;
            maxHp = 5;
            minDamage = 1;
            maxDamage = 3;
        }
        monsterSpeed = Random.Range(minSpeed, maxSpeed);
        monsterHp = Random.Range(minHp, maxHp);
        monsterDamage = Random.Range(minDamage, maxDamage);
        monster.speed = monsterSpeed;
        monster.Hp = monsterHp;
        monster.Damage = monsterDamage;
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
