using System.Collections;
using UnityEngine;

public class LongEnemy : MonoBehaviour
{
    [Header("���� �Ӽ�")]
    public float speed;
    public float Hp;
    public float maxHp;
    public float Damage;
    public EnemyType monsterType = EnemyType.Enemy1;

    [Header("�ൿ ���� ����")]
    public StateType stateType = StateType.Idle;
    private bool isGrounded = false;
    public Transform groundCheck;
    private Rigidbody2D rigidbody;
    public Transform wallCheckRight;
    public Transform wallCheckLeft;
    private bool isWall = false;
    private bool isDead = false;

    [Header("���� �� ���� ����")]
    public Transform player;
    public float attackRange = 5f;
    public bool isAttacking = false;

    [Header("���� �ǵ��")]
    private Color originalColor;
    private Renderer objectRenderer;
    public float colorChangeDuration = 0.5f;

    [Header("���� ����")]
    private Vector3 startPos;
    private int direction = 1;
    public float maxDistance = 3.0f;

    [Header("ü�¹�")]
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

    // ----- ���� �̵� ó�� -----
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

    // ----- �浹 ó�� -----
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerAttack"))
        {
            TakeDamage(1);
            collision.gameObject.GetComponentInParent<PlayerController>().TakeAttack();
        }
    }

    // ----- �ǰ� ó�� -----
    public void TakeDamage(float amount)
    {
        StartCoroutine(ChangeColorTemporarily());
        Hp = Mathf.Max(0, Hp - amount);
        healthBar.UpdateHealthBar(Hp, maxHp);
        StartCoroutine(Die());
    }

    // ----- ��� ó�� -----
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

    // ----- �ǰ� �� ���� ��ȭ -----
    IEnumerator ChangeColorTemporarily()
    {
        SoundManager.Instance.PlaySFX(SFXType.Hit);
        objectRenderer.material.color = Color.red;
        yield return new WaitForSeconds(colorChangeDuration);
        objectRenderer.material.color = originalColor;
    }

    public void ThrowBullet()
    {
        GameObject enemyBullet = BulletPool.Instance.Get("EnemyBullet");
        enemyBullet.transform.position = transform.position;
        enemyBullet.GetComponent<EnemyBullet>().SetBullet(player.position);
    }

    // ----- ���� ��ƾ -----
    IEnumerator AttackRoutine()
    {
        isAttacking = true;
        GetComponent<EnemyAni>().Attack();
        yield return new WaitForSeconds(3.0f);
        isAttacking = false;
    }
}
