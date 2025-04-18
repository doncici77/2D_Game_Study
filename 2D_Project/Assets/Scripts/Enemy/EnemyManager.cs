using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Processors;

// 몬스터 상태 Enum 정의
public enum StateType
{
    Idle, PatrolWalk, PatrolRun, ChaseWalk, ChaseRun, StrongAttack, Attack
}

public class EnemyManager : MonoBehaviour
{
    [Header("몬스터 속성")]
    public float speed;
    public float Hp;
    public float maxHp;
    public float Damage;
    public EnemyType monsterType = EnemyType.Enemy1;

    [Header("행동 상태 관련")]
    public StateType stateType = StateType.Idle;
    private bool isGrounded = false;
    public Transform groundCheck;
    private Rigidbody2D rigidbody;
    public Transform wallCheckRight;
    public Transform wallCheckLeft;
    private bool isWall = false;
    private bool isDead = false;

    [Header("추적 및 공격 관련")]
    public Transform player;
    public float chaseRange = 5.0f;
    public float attackRange = 1.5f;
    public bool isAttacking = false;
    public Collider2D attackLeft;
    public Collider2D attackRight;

    [Header("색상 피드백")]
    private Color originalColor;
    private Renderer objectRenderer;
    public float colorChangeDuration = 0.5f;

    [Header("순찰 관련")]
    private Vector3 startPos;
    private int direction = 1;
    public float maxDistance = 3.0f;

    [Header("체력바")]
    private HealthBar healthBar;

    void Start()
    {
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
        if (isDead)
        {
            return;
        }

        GroundCheck();

        WallCheck();

        if (monsterType == EnemyType.None || player == null)
        {
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        
        if (distanceToPlayer <= attackRange && !isAttacking)
        {
            StartCoroutine(AttackRoutine());
            return;
        }
        else if (isAttacking)
        {
            return;
        }

        if (distanceToPlayer <= chaseRange)
        {
            if (transform.position.x > player.position.x)
            {
                direction = -1;
                GetComponent<SpriteRenderer>().flipX = true;
            }
            else if (transform.position.x < player.position.x)
            {
                direction = 1;
                GetComponent<SpriteRenderer>().flipX = false;
            }

            transform.position += new Vector3(speed * direction * Time.deltaTime, 0, 0);
            return;
        }

        PatrolMovement();
    }

    void GroundCheck()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, LayerMask.GetMask("Ground"));

        if (isGrounded)
        {
            rigidbody.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        }
        else
        {
            rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    void WallCheck()
    {
        if (direction == 1)
        {
            isWall = Physics2D.OverlapCircle(wallCheckRight.position, 0.1f, LayerMask.GetMask("Ground"));

            if (isWall)
            {
                direction = -1;
                GetComponent<SpriteRenderer>().flipX = true;
            }
            else
            {
                isWall = false;
            }
        }
        else if (direction == -1)
        {
            isWall = Physics2D.OverlapCircle(wallCheckLeft.position, 0.1f, LayerMask.GetMask("Ground"));

            if (isWall)
            {
                direction = 1;
                GetComponent<SpriteRenderer>().flipX = false;
            }
            else
            {
                isWall = false;
            }
        }
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

        if (collision.CompareTag("Player") && isAttacking)
        {
            collision.gameObject.GetComponent<PlayerController>().TakeDamage();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isAttacking)
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
        StartCoroutine(Die());
    }

    // ----- 사망 처리 -----
    IEnumerator Die()
    {
        if (Hp <= 0)
        {
            PlayerStats.Instance.Killed();
            GetComponent<Collider2D>().enabled = false;
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            GetComponent<EnemyAni>().Dead();
            isDead = true;

            yield return new WaitForSeconds(1.5f);

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

    public void AttackCollision()
    {
        if (direction == 1)
        {
            attackRight.enabled = true;
        }
        else
        {
            attackLeft.enabled = true;
        }
    }

    public void EndAttackCollision()
    {
        attackRight.enabled = false;
        attackLeft.enabled = false;
    }

    // ----- 공격 루틴 -----
    IEnumerator AttackRoutine()
    {
        isAttacking = true;
        GetComponent<EnemyAni>().Attack();
        yield return new WaitForSeconds(1.0f);
        isAttacking = false;
    }
}

