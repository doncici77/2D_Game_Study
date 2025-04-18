using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

// 몬스터 종류 정의
public enum EnemyType
{
    None,
    Enemy1,
    Enemy2,
    Enemy3
}

public class AIManager : MonoBehaviour
{
    [Header("몬스터 프리팹 및 소환 수")]
    public GameObject monsterPrefab;        // 생성할 몬스터 프리팹
    public int enemyCount = 5;              // 생성할 몬스터 수

    [Header("랜덤 소환 범위 (소환 지점이 없을 경우)")]
    public float spawnRangeX = 10.0f;       // X 축 소환 범위
    public float spawnRangeY = 5.0f;        // Y 축 소환 범위

    [Header("고정 소환 위치 (옵션)")]
    public Transform[] spawnPoints;         // 미리 지정한 소환 지점들

    void Start()
    {
        SpawnEnemies();
    }

    /// <summary>
    /// 몬스터들을 지정된 위치 또는 랜덤 위치에 소환
    /// </summary>
    void SpawnEnemies()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            Vector2 spawnPosition;

            // 지정된 소환 지점이 있다면 그 중 랜덤 선택
            if (spawnPoints.Length > 0)
            {
                int randomIndex = Random.Range(0, spawnPoints.Length);
                spawnPosition = spawnPoints[randomIndex].position;
            }
            else
            {
                // 지정된 지점이 없다면 범위 내에서 랜덤 좌표 생성
                float randomX = Random.Range(-spawnRangeX, spawnRangeX);
                float randomY = Random.Range(-spawnRangeY, spawnRangeY);
                spawnPosition = new Vector2(randomX, randomY);
            }

            // 몬스터 생성 및 스탯 세팅
            GameObject monsterObj = Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);
            EnemyManager enemy = monsterObj.GetComponent<EnemyManager>();
            SetMonsterStats(enemy);
        }
    }

    /// <summary>
    /// 몬스터 타입에 따라 랜덤한 스탯 부여
    /// </summary>
    /// <param name="monster">스탯을 설정할 몬스터</param>
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
