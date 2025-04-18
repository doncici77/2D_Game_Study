using UnityEngine;

public class EnemyAni : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Attack()
    {
        animator.SetTrigger("attack");
    }

    public void Dead()
    {
        animator.SetTrigger("Dead");
        SoundManager.Instance.PlaySFX(SFXType.EnemyDead);
    }
}
