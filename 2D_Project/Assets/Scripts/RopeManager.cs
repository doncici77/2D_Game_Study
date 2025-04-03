using UnityEngine;

public class RopeManager : MonoBehaviour
{
    public GameObject testCircle;

    void Start()
    {

    }

    void Update()
    {
        //카메라 기준 마우스 포인터 위치
        Vector3 point = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));

        if (Input.GetMouseButton(0))
        {
            Debug.Log(point.ToString());
            Vector2 direction = point - transform.position;
            Debug.Log("point - transform.position" + direction.normalized);

            Debug.DrawRay(transform.position, direction, Color.red, 1.0f);

            testCircle.transform.position = Vector2.MoveTowards(testCircle.transform.position, new Vector2(point.x, point.y), Time.deltaTime * 3);
        }
        else
        {
            testCircle.transform.position = Vector2.MoveTowards(testCircle.transform.position, transform.position, Time.deltaTime * 3);
        }
    }
}
