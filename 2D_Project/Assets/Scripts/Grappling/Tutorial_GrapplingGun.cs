using UnityEngine;

public class Tutorial_GrapplingGun : MonoBehaviour
{
    public Tutorial_GrapplingRope grappleRope;

    [Header("Layers Settings:")]
    [SerializeField] private int grappableLayerNumber = 9;

    public Camera m_camera;
    public Transform gunHolder;
    public Transform gunPivot;
    public Transform firePoint;
    public SpringJoint2D m_springJoint2D;
    public Rigidbody2D m_rigidbody;

    [Header("Distance:")]
    public bool hasMaxDistance = false;
    public float maxDistnace = 20;

    [Header("Launching:")]
    public float launchSpeed = 1;

    [Header("No Launch To Point")]
    public float targetDistance = 3;

    [HideInInspector] public Vector2 grapplePoint;
    [HideInInspector] public Vector2 grappleDistanceVector;

    public float impulseForce = 5f; // 순간 힘의 세기
    private bool canImpulse = true;
    private TrailRenderer trail;

    private void Start()
    {
        grappleRope.enabled = false;
        m_springJoint2D.enabled = false;
        trail = gunHolder.gameObject.GetComponent<TrailRenderer>();
    }

    private void Update()
    {
        if (PlayerStats.Instance.isDead)
        {
            return;
        }

        if (PlayerStats.Instance.skillType == SkillType.Grappling)
        {
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                SetGrapplePoint();
            }
            else if (Input.GetKey(KeyCode.Mouse1))
            {
                if (grappleRope.enabled)
                {
                    trail.emitting = true;

                    RotateGun(grapplePoint);

                    // E 키: 오른쪽 방향으로 힘
                    if (Input.GetKeyDown(KeyCode.E) && canImpulse)
                    {
                        m_rigidbody.AddForce(Vector2.right * impulseForce, ForceMode2D.Impulse);
                        canImpulse = false;
                    }

                    // Q 키: 왼쪽 방향으로 힘
                    if (Input.GetKeyDown(KeyCode.Q) && canImpulse)
                    {
                        m_rigidbody.AddForce(Vector2.left * impulseForce, ForceMode2D.Impulse);
                        canImpulse = false;
                    }
                }
                else
                {
                    Vector2 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);
                    RotateGun(mousePos);
                }
            }
            else if (Input.GetKeyUp(KeyCode.Mouse1))
            {
                grappleRope.enabled = false;
                m_springJoint2D.enabled = false;
                trail.emitting = false;

                canImpulse = true;
            }
            else
            {
                Vector2 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);
                RotateGun(mousePos);
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            grappleRope.enabled = false;
            m_springJoint2D.enabled = false;
            trail.emitting = false;

            canImpulse = true;
        }
    }

    public void StopGrapple()
    {
        grappleRope.enabled = false;
        m_springJoint2D.enabled = false;
        trail.emitting = false;

        canImpulse = true;
    }

    void RotateGun(Vector3 lookPoint)
    {
        Vector3 distanceVector = lookPoint - gunPivot.position;

        float angle = Mathf.Atan2(distanceVector.y, distanceVector.x) * Mathf.Rad2Deg;
        gunPivot.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void SetGrapplePoint()
    {
        Vector2 distanceVector = m_camera.ScreenToWorldPoint(Input.mousePosition) - gunPivot.position;
        if (Physics2D.Raycast(firePoint.position, distanceVector.normalized))
        {
            RaycastHit2D _hit = Physics2D.Raycast(firePoint.position, distanceVector.normalized);
            if (_hit.transform.gameObject.layer == grappableLayerNumber)
            {
                if (Vector2.Distance(_hit.point, firePoint.position) <= maxDistnace || !hasMaxDistance)
                {
                    grapplePoint = _hit.point;
                    grappleDistanceVector = grapplePoint - (Vector2)gunPivot.position;
                    grappleRope.enabled = true;

                    SoundManager.Instance.PlaySFX(SFXType.Grappling);
                }
            }
        }
    }

    public void Grapple()
    {
        m_springJoint2D.autoConfigureDistance = false;

        m_springJoint2D.distance = targetDistance;

        m_springJoint2D.connectedAnchor = grapplePoint;
        m_springJoint2D.enabled = true;

        ParticleManager.Instance.ParticlePlay(ParticleType.Grapple, grapplePoint, new Vector3(5, 5, 5));
    }

    private void OnDrawGizmosSelected()
    {
        if (firePoint != null && hasMaxDistance)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(firePoint.position, maxDistnace);
        }
    }
}
