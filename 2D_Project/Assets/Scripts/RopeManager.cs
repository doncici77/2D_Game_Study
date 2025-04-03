using UnityEngine;

public class RopeManager : MonoBehaviour
{
    public LineRenderer lineRenderer;  // 로프를 그리는 LineRenderer
    public float maxRopeLength = 10f;  // 로프의 최대 길이
    public LayerMask ropeLayer;  // 로프가 닿을 수 있는 레이어 (벽, 천장 등)
    public Rigidbody2D rb;  // 플레이어의 Rigidbody2D
    public float swingForce = 5f;  // 매달릴 때 힘의 크기
    public float ropeDamping = 0.99f;  // 감속 효과 (공중에서 흔들릴 때 적용)

    private Vector2 ropeEnd;  // 로프가 닿는 위치
    private bool isSwinging = false;  // 현재 매달린 상태인지 여부

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 마우스 왼쪽 버튼 클릭 시 로프 발사
        {
            ShootRope();
        }

        if (Input.GetMouseButtonUp(0)) // 마우스를 떼면 로프 해제
        {
            ReleaseRope();
        }

        if (isSwinging)
        {
            ApplySwingForce(); // 매달려 있는 동안 힘 적용
        }
    }

    /// <summary>
    /// 마우스를 클릭하면 로프를 발사해서 닿는 위치에 매달린다.
    /// </summary>
    void ShootRope()
    {
        // 마우스 클릭 위치를 월드 좌표로 변환
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0; // 2D 환경이므로 Z 좌표를 0으로 고정

        // Raycast를 사용하여 마우스 방향으로 로프를 발사
        RaycastHit2D hit = Physics2D.Raycast(transform.position, (mousePos - transform.position).normalized, maxRopeLength, ropeLayer);

        if (hit.collider != null) // 충돌이 발생하면
        {
            ropeEnd = hit.point; // 로프의 끝점 설정
            isSwinging = true; // 매달린 상태로 변경
            lineRenderer.enabled = true; // 로프 시각화 활성화

            // 중력을 제거하여 자연스럽게 매달리도록 설정
            rb.gravityScale = 0f;

            // 로프의 방향을 구하고, 초기 힘을 적용하여 흔들리도록 함
            Vector2 swingDirection = (ropeEnd - (Vector2)transform.position).normalized;
            rb.linearVelocity = swingDirection * swingForce;
        }
    }

    /// <summary>
    /// 로프에 매달려 있는 동안 힘을 적용하여 흔들리게 한다.
    /// </summary>
    void ApplySwingForce()
    {
        // 현재 위치와 로프 끝점(매달린 지점) 사이의 방향을 구함
        Vector2 direction = (ropeEnd - (Vector2)transform.position).normalized;

        // 힘을 추가로 적용하여 자연스럽게 흔들리는 효과를 줌
        rb.linearVelocity += direction * swingForce * Time.deltaTime;

        // 감속 효과를 적용하여 무한히 흔들리는 것을 방지
        rb.linearVelocity *= ropeDamping;

        // 로프를 그리기 (위치 업데이트)
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, ropeEnd);

        // 디버그용으로 선을 그려서 Raycast를 시각적으로 확인할 수 있도록 함
        Debug.DrawLine(transform.position, ropeEnd, Color.red);
    }

    /// <summary>
    /// 마우스를 떼면 로프에서 손을 놓는다.
    /// </summary>
    void ReleaseRope()
    {
        isSwinging = false; // 매달린 상태 해제
        lineRenderer.enabled = false; // 로프 숨기기
        rb.gravityScale = 1f; // 중력을 다시 활성화하여 떨어지게 만듦
    }
}
