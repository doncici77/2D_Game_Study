using UnityEngine;

public enum EnemyType
{
    None,
    Enemy1,
    Enemy2,
    Enemy3
}

public class AIManager : MonoBehaviour
{
    public GameObject monsterPrefab;
    public float spawnRangeX = 10.0f;
    public float spawnRangeY = 5.0f;
    public int enemyCount = 5;
    public Transform[] spawnPoints;

    void Start()
    {
        SpawnEnemies();
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            Vector2 spawnPosition;
            if (spawnPoints.Length > 0)
            {
                int randomIndex = Random.Range(0, spawnPoints.Length);
                spawnPosition = spawnPoints[randomIndex].position;
            }
            else
            {
                float randomX = Random.Range(-spawnRangeX, spawnRangeX);
                float randomY = Random.Range(-spawnRangeY, spawnRangeY);
                spawnPosition = new Vector2(randomX, randomY);
            }

            GameObject monsterObj = Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);
            EnemyManager enemy = monsterObj.GetComponent<EnemyManager>();
            SetMonsterStats(enemy);
        }
    }

    void SetMonsterStats(EnemyManager monster)
    {
        float minSpeed = 1f, maxSpeed = 1f;
        float minHp = 1f, maxHp = 1f;
        float minDamage = 1f, maxDamage = 1f;

        switch (monster.monsterType)
        {
            case EnemyType.Enemy1:
                minSpeed = 1; maxSpeed = 5;
                minHp = 1; maxHp = 10;
                minDamage = 1; maxDamage = 10;
                break;
            case EnemyType.Enemy2:
                minSpeed = 0.5f; maxSpeed = 3.0f;
                minHp = 5; maxHp = 12;
                minDamage = 3; maxDamage = 12;
                break;
            case EnemyType.Enemy3:
                minSpeed = 3.0f; maxSpeed = 7.0f;
                minHp = 1; maxHp = 5;
                minDamage = 1; maxDamage = 3;
                break;
        }

        monster.speed = Random.Range(minSpeed, maxSpeed);
        monster.Hp = Random.Range(minHp, maxHp);
        monster.maxHp = monster.Hp;
        monster.Damage = Random.Range(minDamage, maxDamage);
    }
}

