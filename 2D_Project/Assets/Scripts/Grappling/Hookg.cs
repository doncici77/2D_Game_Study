using UnityEngine;

public class Hookg : MonoBehaviour
{
    Gappling2 grappling;
    public DistanceJoint2D joint2D;
    void Start()
    {
        grappling = GameObject.Find("Player").GetComponent<Gappling2>();
        joint2D = GetComponent<DistanceJoint2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ring"))
        {
            joint2D.enabled = true;
            grappling.isAttach = true;
        }
    }
}
