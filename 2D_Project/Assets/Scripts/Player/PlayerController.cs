using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerMovement movement;
    private PlayerAttack attack;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    public bool isInvincible = false;
    public float invincibilityDuration = 1.0f;
    public float knockbackForce = 5.0f;
    private Rigidbody2D rb;
    private bool isKnockback = false;
    public float knockbackDuration = 0.2f;

    private Vector3 startPlayerPos;
    private bool isPaused = false;
    public GameObject pauseMenuUI;

    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.1f;
    private Vector3 originalPos;

    public float delayTime = 0.3f;

    private bool canGoingNextScene = false;
    private string nextSceneName;

    [Header("카메라 쉐이크 설정")]
    public CinemachineImpulseSource impulseSource;

    public DungeonGenerator dungeonGenerator;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        attack = GetComponent<PlayerAttack>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        startPlayerPos = transform.position;

        if (Camera.main != null)
        {
            originalPos = Camera.main.transform.localPosition;
        }
    }

    void Update()
    {
        if (PlayerStats.Instance.isDead)
        {
            return;
        }

        if (!isKnockback)
        {
            movement.HandleMovement();
        }

        attack.PerformAttack();

        if (Input.GetKeyDown(KeyCode.W) && canGoingNextScene)
        {
            SceneController.Instance.StartSeneTransition(nextSceneName);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ReGame();
            }
            else
            {
                Pause();
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (PlayerStats.Instance.skillType == SkillType.Grappling)
            {
                PlayerStats.Instance.ChangeSkill(SkillType.Dash);
            }
            else if (PlayerStats.Instance.skillType == SkillType.Dash)
            {
                PlayerStats.Instance.ChangeSkill(SkillType.Grappling);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PlayerStats.Instance.ChangeWeapon(WeaponType.Long);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PlayerStats.Instance.ChangeWeapon(WeaponType.Short);
        }
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ReGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1.0f;
        isPaused = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            GameManager.instance.AddCoin(10);
            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("DeathZone"))
        {
            SoundManager.Instance.PlaySFX(SFXType.TakeDamage);
            transform.position = startPlayerPos;
        }

        if (collision.CompareTag("Portal"))
        {
            nextSceneName = collision.gameObject.name;
            canGoingNextScene = true;
        }

        if (collision.CompareTag("LeftPortal"))
        {
            dungeonGenerator.GenerateDungeon(PortalDir.left);
        }
        else if (collision.CompareTag("RightPortal"))
        {
            dungeonGenerator.GenerateDungeon(PortalDir.right);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Portal"))
        {
            canGoingNextScene = false;
        }
    }

    public void TakeDamage()
    {
        if (!isInvincible)
        {
            GenerateCameraImpulse();
            SoundManager.Instance.PlaySFX(SFXType.TakeDamage);
            animator.SetTrigger("Hit");
            ParticleManager.Instance.ParticlePlay(ParticleType.PlayerDamage, transform.position, new Vector3(6, 6, 6));
            StartCoroutine(Invincibility());

            Vector2 knockbackDirection = transform.localScale.x < 0 ? Vector2.right : Vector2.left;
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
            StartCoroutine(KnockbackCoroutine());

            PlayerStats.Instance.TakeDamage(10);
        }
    }

    public void TakeAttack()
    {
        StartCoroutine(DelayTime());
        GenerateCameraImpulse();
    }

    IEnumerator DelayTime()
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(delayTime);
        Time.timeScale = 1f;
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        Camera.main.GetComponent<CinemachineBrain>().enabled = false;
        if (Camera.main == null)
        {
            yield break;
        }

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = UnityEngine.Random.Range(-1f, 1f) * magnitude;
            float y = UnityEngine.Random.Range(-1f, 1f) * magnitude;

            Camera.main.transform.localPosition = new Vector3(Camera.main.transform.localPosition.x, originalPos.y + y, -10);

            elapsed += Time.deltaTime;

            yield return null;
        }

        Camera.main.transform.localPosition = originalPos;
        Camera.main.GetComponent<CinemachineBrain>().enabled = true;
    }

    private void GenerateCameraImpulse()
    {
        if (impulseSource != null)
        {
            impulseSource.GenerateImpulse();
        }
        else
        {
            Debug.LogWarning("ImpulseSource가 연결이 안되어있습니다.");
        }
    }

    IEnumerator Invincibility()
    {
        isInvincible = true;

        float elapsedTime = 0f;
        float blinkInterval = 0.2f;

        Color originalColor = spriteRenderer.color;

        StartCoroutine(DelayTime());

        while (elapsedTime < invincibilityDuration)
        {
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.4f);
            yield return new WaitForSeconds(blinkInterval);
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1.0f);
            yield return new WaitForSeconds(blinkInterval);
            elapsedTime += blinkInterval * 2;
        }
        spriteRenderer.color = originalColor;
        isInvincible = false;
    }

    IEnumerator KnockbackCoroutine()
    {
        isKnockback = true;
        yield return new WaitForSeconds(knockbackDuration);
        isKnockback = false;
    }
}
