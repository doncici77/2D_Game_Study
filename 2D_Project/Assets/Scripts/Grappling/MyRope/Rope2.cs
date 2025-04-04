using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Rope2 : MonoBehaviour
{
    public Image crossHair;
    public LayerMask obstacleMask; // ��ֹ� ���̾� ����ũ
    public float rotationSpeed = 1.5f;

    private Rigidbody2D rb;
    private Vector2 circleCenter;
    private float circleRadius;
    private bool isCircleActive = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // ���콺 ������ ��ġ ���
        Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.nearClipPlane
        ));
        crossHair.transform.position = point;

        // ��Ŭ������ �� ���� / ����
        if (Input.GetButtonDown("Fire2"))
        {
            if (!isCircleActive)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, point - transform.position, Mathf.Infinity, 1 << 3);
                if (hit.collider != null)
                {
                    circleCenter = hit.point;
                    circleRadius = Vector2.Distance(transform.position, circleCenter);
                    isCircleActive = true;
                }
            }
            else
            {
                isCircleActive = false;
            }
        }
    }

    private void FixedUpdate()
    {
        if (isCircleActive)
        {
            MoveAlongCircleEdgeWithCollision();
        }
    }

    private void MoveAlongCircleEdgeWithCollision()
    {
        Vector2 direction = (Vector2)transform.position - circleCenter;
        float currentAngle = Mathf.Atan2(direction.y, direction.x);

        float input = Input.GetAxis("Horizontal");
        float angleChange = input * rotationSpeed * Time.fixedDeltaTime;
        float newAngle = currentAngle + angleChange;

        Vector2 targetPos = circleCenter + new Vector2(Mathf.Cos(newAngle), Mathf.Sin(newAngle)) * circleRadius;

        Vector2 moveDirection = targetPos - rb.position;

        // �浹 �˻�
        RaycastHit2D hit = Physics2D.Raycast(rb.position, moveDirection.normalized, moveDirection.magnitude, obstacleMask);
        if (hit.collider == null)
        {
            rb.MovePosition(targetPos); // �浹 ������ �̵�
        }
    }

    private void OnDrawGizmos()
    {
        if (isCircleActive)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(circleCenter, circleRadius);
        }
    }
}
