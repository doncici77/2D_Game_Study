using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerMovement movement;
    private PlayerAttack attack;
    private PlayerHealth health;

    private Vector3 startPlayerPos;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        attack = GetComponent<PlayerAttack>();
        health = GetComponent<PlayerHealth>();
    }

    void Start()
    {
        startPlayerPos = transform.position;
    }

    void Update()
    {
        movement.HandleMovement();
        attack.PerformAttack();
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
    }
}
