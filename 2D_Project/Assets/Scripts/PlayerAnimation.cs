using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    public Transform attackPivot;
    public Transform attackPos;

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

    public void SetRunnig(bool isRunnig)
    {
        animator.SetBool("IsRun", isRunnig);
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
        if (transform.localScale.x == -6)
        {
            ParticleManager.Instance.ParticlePlay(ParticleType.PlayerAttack, attackPos.position, new Vector3(-5, 5, 5), attackPivot.eulerAngles);
        }
        else
        {
            ParticleManager.Instance.ParticlePlay(ParticleType.PlayerAttack, attackPos.position, new Vector3(5, 5, 5), attackPivot.eulerAngles);
        }
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
