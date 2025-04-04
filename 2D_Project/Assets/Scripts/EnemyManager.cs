using System.Collections;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private Color originalColor;
    private Renderer objectRenderer;
    public float colorChangeDuration = 0.5f;
    private float ZombieHp = 10.0f;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        originalColor = objectRenderer.material.color;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "AttackPos")
        {
            StartCoroutine(ChangeColorTemporatily());
        }
    }

    IEnumerator ChangeColorTemporatily()
    {
        // 사운드 메니저 추가
        objectRenderer.material.color = Color.red;
        yield return new WaitForSeconds(colorChangeDuration);
        objectRenderer.material.color = originalColor;
    }
}
