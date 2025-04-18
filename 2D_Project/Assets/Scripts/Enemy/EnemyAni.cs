using UnityEngine;

public class EnemyAni : MonoBehaviour
{
    private Animator animator;

    public void Attack()
    {
        animator.SetTrigger("attack");
    }
}
