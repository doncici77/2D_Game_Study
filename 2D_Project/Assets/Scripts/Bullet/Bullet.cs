using UnityEngine;

public class Bullet : MonoBehaviour
{
    private bool move = false;
    private Vector3 target;

    void Update()
    {
        if (move)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * 50f);
        }
    }

    public void SetBullet(Vector3 getTarget)
    {
        target = getTarget;
        move = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 3)
        {
            BulletPool.Instance.ReturnToPool("Bullet",this.gameObject);
        }

        if(collision.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<EnemyManager>().TakeDamage(1);
            BulletPool.Instance.ReturnToPool("Bullet", this.gameObject);
        }
        else if(collision.CompareTag("LongEnemy"))
        {
            collision.gameObject.GetComponent<LongEnemy>().TakeDamage(1);
            BulletPool.Instance.ReturnToPool("Bullet", this.gameObject);
        }
    }
}
