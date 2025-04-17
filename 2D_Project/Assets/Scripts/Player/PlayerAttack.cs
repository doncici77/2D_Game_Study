using System;
using System.Collections;
using Unity.Cinemachine;
using Unity.Mathematics;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private PlayerAnimation playerAnimation;
    private Animator animator;
    private bool isAttacking = false;
    public Camera m_camera;
    public Transform attackPivot;

    [Header("�ִϸ��̼� ���� �̸�")]
    public string attackStateName = "Player_Attack";

    void Start()
    {
        playerAnimation = GetComponent<PlayerAnimation>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (PlayerStats.Instance.isDead)
        {
            return;
        }

        Vector2 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);
        RotateGun(mousePos);
    }

    void RotateGun(Vector3 lookPoint)
    {
        Vector3 distanceVector = lookPoint - attackPivot.position;

        float angle = Mathf.Atan2(distanceVector.y, distanceVector.x) * Mathf.Rad2Deg;

        if (transform.localScale.x < 0)
        {
            angle += 180f;
        }

        attackPivot.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public void PerformAttack()
    {
        if (Input.GetButtonDown("Fire1"))
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

        yield return null; // ���� �����ӱ��� ���
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
}
