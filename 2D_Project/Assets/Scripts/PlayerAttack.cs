using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject attackCollider;
    private PlayerAnimation playerAnimation;
    private Animator animator;

    private bool isAttacking = false;

    [Header("�ִϸ��̼� ���� �̸�")]
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

        yield return null; // ���� �÷��ӱ��� ���
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName(attackStateName))
        {
            float animationLength = stateInfo.length;
            yield return new WaitForSeconds(animationLength);
        }
        else
        {
            // ���� �ٸ� �ִϸ��̼� ���� �̸��̸� ���⿡ �߰�
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
