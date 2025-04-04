using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Rope2 : MonoBehaviour
{
    public Image crossHair;
    public float circleRadius = 3f; // 원 반지름
    private Vector3 circleCenter; // 원 중심 좌표
    private bool isInsideCircle = false;

    private void Update()
    {
        // 마우스 위치 변환 (월드 좌표로 변환)
        Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.nearClipPlane
        ));

        // 크로스헤어 위치 업데이트
        crossHair.transform.position = point;

        if (Input.GetButtonDown("Fire2"))
        {
            int groundLayer = 3; // 감지할 레이어 번호
            RaycastHit hit;
            Debug.Log("Fire2 Click");

            if (Physics.Raycast(transform.position, point - transform.position, out hit, Mathf.Infinity, 1 << groundLayer))
            {
                Debug.Log("The ray hit the Ground at: " + hit.point);

                // 원 중심 설정
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
        // 플레이어와 원 중심 사이의 벡터 계산
        Vector3 directionToCenter = transform.position - circleCenter;
        float distanceToCenter = directionToCenter.magnitude;

        // 플레이어가 원 반지름을 초과하면 위치 조정
        if (distanceToCenter > circleRadius)
        {
            Vector3 newPosition = circleCenter + directionToCenter.normalized * circleRadius;
            transform.position = newPosition;
        }
    }
}
