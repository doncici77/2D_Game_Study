using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void TriggerAttack()
    {
        animator.SetTrigger("Attack");
    }

    public void SetWalking(bool iswalk)
    {
        animator.SetBool("IsWalking", iswalk);
    }
}
