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
    public Transform attackPos;
    public GameObject bulletPrfab;
    private GameObject bullet;

    [Header("애니메이션 상태 이름")]
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

    void Shooting()
    {
        // Raycast로 앞에 충돌체 있는지 확인
        Vector2 direction = (attackPos.position - transform.position).normalized;

        int layerMask = 1 << LayerMask.NameToLayer("Ground");

        RaycastHit2D hit = Physics2D.Raycast(attackPos.position, direction, Mathf.Infinity, layerMask);
        Debug.DrawRay(attackPos.position, direction, Color.yellow, 3f);
        Debug.Log("hit : " + hit.transform.gameObject.layer);
        if (hit.transform.gameObject.layer == 3)
        {
            bullet = Instantiate(bulletPrfab, attackPos.position, Quaternion.identity);
            bullet.gameObject.GetComponent<Bullet>().SetBullet(hit.point, true);
        }
    }

    public void PerformAttack()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (PlayerStats.Instance.weaponType == WeaponType.Short)
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
            else if(PlayerStats.Instance.weaponType == WeaponType.Long)
            {
                Shooting();
            }
        }
    }

    IEnumerator AttackColldownByAnimation()
    {
        isAttacking = true;

        yield return null; // 다음 프레임까지 대기
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
}
