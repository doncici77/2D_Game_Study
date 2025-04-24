using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    public EnemyManager enemyManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Ãæµ¹");
        if (collision.CompareTag("Player") && enemyManager.isAttacking)
        {
            collision.gameObject.GetComponent<PlayerController>().TakeDamage();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && enemyManager.isAttacking)
        {
            collision.gameObject.GetComponent<PlayerController>().TakeDamage();
        }
    }
}
