using UnityEngine;

public class DrawCircle : MonoBehaviour
{
    private Tutorial_GrapplingGun grapplingGun;
    public int segments = 100;      // 얼마나 부드럽게 그릴지
    private LineRenderer line;

    void Start()
    {
        grapplingGun = gameObject.GetComponent<Tutorial_GrapplingGun>();

        line = GetComponent<LineRenderer>();
        line.positionCount = segments + 1;
        line.loop = true;
        line.useWorldSpace = true;
    }

    void DrawCircleAroundPlayer()
    {
        Vector3 center = transform.position;

        for (int i = 0; i <= segments; i++)
        {
            float angle = (float)i / segments * Mathf.PI * 2f;
            float x = Mathf.Cos(angle) * grapplingGun.maxDistnace;
            float y = Mathf.Sin(angle) * grapplingGun.maxDistnace;

            line.SetPosition(i, new Vector3(center.x + x, center.y + y, center.z));
        }
    }

    void Update()
    {
        if(PlayerStats.Instance.skillType == SkillType.Grappling)
        {
            line.enabled = true;
            DrawCircleAroundPlayer(); // 플레이어가 움직일 때 따라오게 하려면 필요
        }
        else
        {
            line.enabled = false;
        }
    }
}

