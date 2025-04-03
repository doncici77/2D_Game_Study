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

    public void SetWalking(bool iswalking)
    {
        animator.SetBool("IsWalking", iswalking);
    }

    public void SetFalling(bool isFalling)
    {
        animator.SetBool("IsFalling", isFalling);
    }

    public void SetJumping(bool isJumping)
    {
        animator.SetBool("IsJumping", isJumping);
    }

    public void PlayLanding()
    {
        animator.SetTrigger("Land");
        animator.SetBool("IsJumping", false);
        animator.SetBool("IsFalling", false);
    }

    public void PlaySoundWalk()
    {
        SoundManager.Instance.PlaySFX(SFXType.Walk);
    }

    public void PlaySoundAttack()
    {
        SoundManager.Instance.PlaySFX(SFXType.Attack);
    }

    public void PlaySoundJump()
    {
        SoundManager.Instance.PlaySFX(SFXType.Jump);
    }

    public void PlaySoundLand()
    {
        SoundManager.Instance.PlaySFX(SFXType.Land);
    }
}
