using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Rope2 : MonoBehaviour
{
    public Image crossHair;
    public float circleRadius = 3f; // �� ������
    private Vector3 circleCenter; // �� �߽� ��ǥ
    private bool isInsideCircle = false;

    private void Update()
    {
        // ���콺 ��ġ ��ȯ (���� ��ǥ�� ��ȯ)
        Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.nearClipPlane
        ));

        // ũ�ν���� ��ġ ������Ʈ
        crossHair.transform.position = point;

        if (Input.GetButtonDown("Fire2"))
        {
            int groundLayer = 3; // ������ ���̾� ��ȣ
            RaycastHit hit;
            Debug.Log("Fire2 Click");

            if (Physics.Raycast(transform.position, point - transform.position, out hit, Mathf.Infinity, 1 << groundLayer))
            {
                Debug.Log("The ray hit the Ground at: " + hit.point);

                // �� �߽� ����
                circleCenter = hit.point;
                isInsideCircle = true;
            }
        }

        if (isInsideCircle)
        {
            RestrictPlayerMovement();
        }
    }

    private void RestrictPlayerMovement()
    {
        // �÷��̾�� �� �߽� ������ ���� ���
        Vector3 directionToCenter = transform.position - circleCenter;
        float distanceToCenter = directionToCenter.magnitude;

        // �÷��̾ �� �������� �ʰ��ϸ� ��ġ ����
        if (distanceToCenter > circleRadius)
        {
            Vector3 newPosition = circleCenter + directionToCenter.normalized * circleRadius;
            transform.position = newPosition;
        }
    }
}
