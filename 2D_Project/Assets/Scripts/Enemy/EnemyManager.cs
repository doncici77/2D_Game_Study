using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

// 몬스터 상태 Enum 정의
public enum StateType
{
    Idle, PatrolWalk, PatrolRun, ChaseWalk, ChaseRun, StrongAttack, Attack
}

public class EnemyManager : MonoBehaviour
{
    // ----- 몬스터 속성 -----
    public float speed;
    public float Hp;
    public float maxHp;
    public float Damage;
    public EnemyType monsterType = EnemyType.Enemy1;

    // ----- 행동 상태 관련 -----
    public StateType stateType = StateType.Idle;
    private bool isGrounded = false;
    public Transform groundCheck;
    private Rigidbody2D rigidbody;
    private EnemyAni ani;
    public Transform wallCheckRight;
    public Transform wallCheckLeft;
    private bool isWall = false;

    // ----- 추적 및 공격 관련 -----
    public Transform player;
    public float chaseRange = 5.0f;
    public float attackRange = 1.5f;
    public bool isAttacking = false;

    // ----- 색상 피드백 -----
    private Color originalColor;
    private Renderer objectRenderer;
    public float colorChangeDuration = 0.5f;

    // ----- 순찰 관련 -----
    private Vector3 startPos;
    private int direction = 1;
    public float maxDistance = 3.0f;

    // ----- 체력바 -----
    private HealthBar healthBar;

    void Start()
    {
        ani = GetComponent<EnemyAni>();
        rigidbody = GetComponent<Rigidbody2D>();
        healthBar = GetComponentInChildren<HealthBar>();
        objectRenderer = GetComponent<SpriteRenderer>();
        originalColor = objectRenderer.material.color;
        startPos = transform.position;

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
        }
    }

    void Update()
    {
        // ----- 바닥 체크 -----
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, LayerMask.GetMask("Ground"));

        rigidbody.constraints = isGrounded
            ? RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation
            : RigidbodyConstraints2D.FreezeRotation;

        // ----- 벽 체크 및 방향 전환 (순찰용) -----
        Transform wallCheck = direction == 1 ? wallCheckRight : wallCheckLeft;
        isWall = Physics2D.OverlapCircle(wallCheck.position, 0.1f, LayerMask.GetMask("Ground"));
        if (isWall)
        {
            direction *= -1;
            GetComponent<SpriteRenderer>().flipX = direction == -1;
        }

        // ----- 플레이어 없는 경우 행동 중지 -----
        if (monsterType == EnemyType.None || player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // ----- 공격 -----
        if (distanceToPlayer <= attackRange && !isAttacking)
        {
            StopAllCoroutines();
            StartCoroutine(AttackRoutine());
            return;
        }
        else if (isAttacking)
        {
            return;
        }

        // ----- 추적 -----
        if (distanceToPlayer <= chaseRange)
        {
            direction = transform.position.x > player.position.x ? -1 : 1;
            GetComponent<SpriteRenderer>().flipX = direction == -1;

            // 추적 중 벽 감지 시 이동 중지
            bool chaseWallCheck = Physics2D.OverlapCircle(
                direction == 1 ? wallCheckRight.position : wallCheckLeft.position,
                0.1f, LayerMask.GetMask("Ground"));

            if (!chaseWallCheck)
            {
                transform.position += new Vector3(speed * direction * Time.deltaTime, 0, 0);
            }
            return;
        }

        // ----- 순찰 -----
        PatrolMovement();
    }

    // ----- 순찰 이동 처리 -----
    private void PatrolMovement()
    {

        if (transform.position.x > startPos.x + maxDistance)
        {
            direction = -1;
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (transform.position.x < startPos.x - maxDistance)
        {
            direction = 1;
            GetComponent<SpriteRenderer>().flipX = false;
        }

        transform.position += new Vector3(speed * direction * Time.deltaTime, 0, 0);

    }

    // ----- 충돌 처리 -----
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerAttack"))
        {
            TakeDamage(1);
            collision.gameObject.GetComponentInParent<PlayerController>().TakeAttack();
        }

        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().TakeDamage();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().TakeDamage();
        }
    }

    // ----- 피격 처리 -----
    public void TakeDamage(float amount)
    {
        StartCoroutine(ChangeColorTemporarily());
        Hp = Mathf.Max(0, Hp - amount);
        healthBar.UpdateHealthBar(Hp, maxHp);
        Die();
    }

    // ----- 사망 처리 -----
    void Die()
    {
        if (Hp <= 0)
        {
            PlayerStats.Instance.Killed();
            gameObject.SetActive(false);
        }
    }

    // ----- 피격 시 색상 변화 -----
    IEnumerator ChangeColorTemporarily()
    {
        SoundManager.Instance.PlaySFX(SFXType.Hit);
        objectRenderer.material.color = Color.red;
        yield return new WaitForSeconds(colorChangeDuration);
        objectRenderer.material.color = originalColor;
    }

    // ----- 공격 루틴 -----
    IEnumerator AttackRoutine()
    {
        isAttacking = true;
        ani.Attack();
        yield return new WaitForSeconds(1.0f);
        isAttacking = false;
    }
}

