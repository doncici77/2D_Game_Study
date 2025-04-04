using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject attackCollider;
    private PlayerAnimation playerAnimation;
    private Animator animator;

    private bool isAttacking = false;

    [Header("애니메이션 상태 이름")]
    public string attackStateName = "Player_Attack";

    void Start()
    {
        playerAnimation = GetComponent<PlayerAnimation>();
        animator = GetComponent<Animator>();
    }

    public void PerformAttack()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (isAttacking)
            {
                return;
            }

            if (playerAnimation != null)
            {
                playerAnimation.TriggerAttack();
            }

            StartCoroutine(AttackColldownByAnimation());
        }
    }

    IEnumerator AttackColldownByAnimation()
    {
        isAttacking = true;

        yield return null; // 다음 플레임까지 대기
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName(attackStateName))
        {
            float animationLength = stateInfo.length;
            yield return new WaitForSeconds(animationLength);
        }
        else
        {
            // 만약 다른 애니메이션 상태 이름이면 여기에 추가
        }

        isAttacking = false;
    }

    public void OnAttack()
    {
        attackCollider.SetActive(true);
    }

    public void OffAttack()
    {
        attackCollider.SetActive(false);
    }
}
