using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Rope2 : MonoBehaviour
{
    public Image crossHair;
    public LayerMask obstacleMask; // 장애물 레이어 마스크
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
        // 마우스 포인터 위치 계산
        Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.nearClipPlane
        ));
        crossHair.transform.position = point;

        // 우클릭으로 원 고정 / 해제
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

        // 충돌 검사
        RaycastHit2D hit = Physics2D.Raycast(rb.position, moveDirection.normalized, moveDirection.magnitude, obstacleMask);
        if (hit.collider == null)
        {
            rb.MovePosition(targetPos); // 충돌 없으면 이동
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
