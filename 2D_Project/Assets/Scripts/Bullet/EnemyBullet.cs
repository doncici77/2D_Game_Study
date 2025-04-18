using System.Collections;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private bool move = false;
    private Vector3 target;
    public float bulletSpeed = 20f;

    void Update()
    {
        if (move)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * bulletSpeed);
        }

        if(transform.position == target)
        {
            StartCoroutine(DeleteBullet());
        }
    }

    public void SetBullet(Vector3 getTarget)
    {
        target = getTarget;
        move = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.gameObject.GetComponent<PlayerController>().isInvincible == false)
            {
                collision.gameObject.GetComponent<PlayerController>().TakeDamage();
                BulletPool.Instance.ReturnToPool("EnemyBullet", this.gameObject);
                gameObject.SetActive(false);
            }
        }
    }

    IEnumerator DeleteBullet()
    {
        yield return new WaitForSeconds(0.3f);
        BulletPool.Instance.ReturnToPool("EnemyBullet", this.gameObject);
        gameObject.SetActive(false);
    }
}
