using UnityEngine;

public class RopeCreate : MonoBehaviour
{
    public GameObject rope;
    public int ropeCount = 0;
    public Rigidbody2D pointRig;
    private FixedJoint2D joint2D;

    void Start()
    {
        for(int i = 0;  i < ropeCount; i++)
        {
            FixedJoint2D currentJoint = Instantiate(rope, transform).GetComponent<FixedJoint2D>();

            if(i == 0)
            {
                currentJoint.connectedBody = pointRig;
                currentJoint.transform.localPosition = new Vector3(0, -0.2f, 0);
            }
            else
            {
                currentJoint.connectedBody = joint2D.GetComponent<Rigidbody2D>();
                currentJoint.transform.localPosition = new Vector3(0, (i + 1) * -2, 0);
            }

            joint2D = currentJoint;

            if(i == (ropeCount - 1))
            {
                currentJoint.GetComponent<Rigidbody2D>().mass = 10;
                currentJoint.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }

    void Update()
    {
        
    }
}
