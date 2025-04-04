using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerMovement movement;
    private PlayerAttack attack;
    private PlayerHealth health;

    private Vector3 startPlayerPos;
    private bool isPaused = false;
    public GameObject pauseMenuUI;

    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.1f;
    private Vector3 originalPos;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        attack = GetComponent<PlayerAttack>();
        health = GetComponent<PlayerHealth>();
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
        movement.HandleMovement();
        attack.PerformAttack();

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
        if(collision.CompareTag("Coin"))
        {
            GameManager.instance.AddCoin(10);
            Destroy(collision.gameObject);
        }
        
        if(collision.CompareTag("DeathZone"))
        {
            SoundManager.Instance.PlaySFX(SFXType.TakeDamage);
            transform.position = startPlayerPos;
        }

        if(collision.CompareTag("Enemy"))
        {
            StartCoroutine(Shake(shakeDuration, shakeMagnitude));
        }
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
}
