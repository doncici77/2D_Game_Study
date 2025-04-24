using UnityEngine;

public class EnemyAttackPos : MonoBehaviour
{
    public Transform enemyPos;

    void Update()
    {
        transform.position = enemyPos.position;
    }
}
