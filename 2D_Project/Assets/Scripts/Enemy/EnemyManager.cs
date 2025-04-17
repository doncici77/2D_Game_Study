using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public enum StateType
{
    Idle, PatrolWalk, PatrolRun, ChaseWalk, ChaseRun, StrongAttack, Attack
}

public class EnemyManager : MonoBehaviour
{
    private Color originalColor;
    private Renderer objectRenderer;
    public float colorChangeDuration = 0.5f;
    public float speed;
    public float Hp;
    public float maxHp;
    public float Damage;
    public float maxDistance = 3.0f;
    private Vector3 startPos;
    private int direction = 1;
    public GroundType currentGroundType;
    public EnemyType monsterType = EnemyType.Enemy1;
    public StateType stateType = StateType.Idle;

    public Transform player;
    public float chaseRange = 5.0f;
    public float attackRange = 1.5f;
    public bool isAttacking = false;

    private float stateChanageInterval = 3.0f;
    private Coroutine stateChangeRoutine;

    private HealthBar healthBar;

    void Start()
    {
        healthBar = GetComponentInChildren<HealthBar>();
        objectRenderer = GetComponent<SpriteRenderer>();
        originalColor = objectRenderer.material.color;
        startPos = transform.position;

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
        }

        stateChangeRoutine = StartCoroutine(RandomStateChanger());
    }

    void Update()
    {
        if (monsterType != EnemyType.None)
        {
            if (player == null) return;

            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if (distanceToPlayer <= attackRange && !isAttacking)
            {
                if (stateType != StateType.Attack)
                {
                    StopAllCoroutines();
                    stateType = StateType.StrongAttack;
                    StartCoroutine(AttackRoutine());
                }
                return;
            }

            if (distanceToPlayer <= chaseRange)
            {
                if (stateType != StateType.ChaseWalk && stateType != StateType.ChaseRun)
                {
                    if (stateChangeRoutine != null)
                        StopCoroutine(stateChangeRoutine);

                    int chaseType = Random.Range(0, 2);
                    stateType = chaseType == 0 ? StateType.ChaseWalk : StateType.ChaseRun;
                }

                Vector3 directionToPlayer = (player.position - transform.position).normalized;
                float chaseSpeed = stateType == StateType.ChaseRun ? speed * 2 : speed;
                transform.position += directionToPlayer * chaseSpeed * Time.deltaTime;
                return;
            }

            if ((stateType == StateType.ChaseWalk || stateType == StateType.ChaseRun) && distanceToPlayer > chaseRange)
            {
                stateType = StateType.Idle;
                if (stateChangeRoutine == null)
                    stateChangeRoutine = StartCoroutine(RandomStateChanger());
            }

            if (stateType == StateType.Attack) return;

            PartrolMovement();
        }
    }

    private void PartrolMovement()
    {
        if (currentGroundType == GroundType.UpGround)
        {
            if (stateType == StateType.PatrolWalk || stateType == StateType.PatrolRun)
            {
                if (transform.position.y > startPos.y + maxDistance) direction = -1;
                else if (transform.position.y < startPos.y - maxDistance) direction = 1;

                float movespeed = stateType == StateType.PatrolRun ? speed * 2 : speed;
                transform.position += new Vector3(0, movespeed * direction * Time.deltaTime, 0);
            }
        }
        else
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
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerAttack"))
        {
            StartCoroutine(ChangeColorTemporatily());
            collision.gameObject.GetComponentInParent<PlayerController>().TakeAttack();

            Hp = Mathf.Max(0, Hp - 1);
            healthBar.UpdateHealthBar(Hp, maxHp);
        }
        else if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().TakeDamage();
        }
        
        if(collision.CompareTag("Bullet"))
        {
            StartCoroutine(ChangeColorTemporatily());

            Hp = Mathf.Max(0, Hp - 1);
            healthBar.UpdateHealthBar(Hp, maxHp);

            Debug.Log("collision : " + collision.name);
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().TakeDamage();
        }
    }

    IEnumerator ChangeColorTemporatily()
    {
        SoundManager.Instance.PlaySFX(SFXType.Hit);
        objectRenderer.material.color = Color.red;
        yield return new WaitForSeconds(colorChangeDuration);
        objectRenderer.material.color = originalColor;
    }

    IEnumerator RandomStateChanger()
    {
        while (true)
        {
            yield return new WaitForSeconds(stateChanageInterval);
            int randomState = Random.Range(0, 3);
            stateType = (StateType)randomState;
        }
    }

    IEnumerator AttackRoutine()
    {
        isAttacking = true;
        yield return new WaitForSeconds(1.0f);
        isAttacking = false;
        stateChangeRoutine = StartCoroutine(RandomStateChanger());
    }
}
