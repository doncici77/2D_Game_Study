using UnityEngine;

public class MousePos : MonoBehaviour
{
    private void Start()
    {
        Cursor.visible = false;
    }

    void Update()
    {
        Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
        transform.position = point;
    }
}
