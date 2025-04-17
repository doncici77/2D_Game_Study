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

    public void SetBullet(Vector3 getTarget, bool isMove)
    {
        target = getTarget;
        move = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 3)
        {
            Destroy(gameObject);
        }
    }
}
