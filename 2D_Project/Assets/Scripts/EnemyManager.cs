using System.Collections;
using UnityEngine;

public enum EnemyType
{
    None,
    Enemy
}

public class EnemyManager : MonoBehaviour
{
    private Color originalColor;
    private Renderer objectRenderer;
    public float colorChangeDuration = 0.5f;
    private float zombieHp = 10.0f;
    public float speed = 2.0f;
    public float maxDistance = 3.0f;
    private Vector3 startPos;
    private int direction = 1;
    public GroundType currentGroundType;
    public EnemyType monsterType = EnemyType.None;

    void Start()
    {
        objectRenderer = GetComponent<SpriteRenderer>();
        originalColor = objectRenderer.material.color;
        startPos = transform.position;
    }

    private void Update()
    {
        if (monsterType != EnemyType.None)
        {
            if (currentGroundType == GroundType.UpGround)
            {
                if (transform.position.y > startPos.y + maxDistance)
                {
                    direction = -1;
                }
                else if (transform.position.y < startPos.y - maxDistance)
                {
                    direction = 1;
                }

                transform.position += new Vector3(0, speed * direction * Time.deltaTime, 0);
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


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerAttack"))
        {
            StartCoroutine(ChangeColorTemporatily());
        }
        else if(collision.CompareTag("Player"))
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
}
