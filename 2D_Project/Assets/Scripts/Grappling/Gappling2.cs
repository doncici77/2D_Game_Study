using UnityEngine;
using UnityEngine.UI;

public class Gappling2 : MonoBehaviour
{
    public LineRenderer line;
    public Transform hook;
    public Vector2 releaseVelocity;
    Vector2 mousedir;

    public bool isHookActive;
    public bool isLineMax;

    public bool isAttach;
    public bool isReleased = false; // �׷��ø� ���� ����
    public float grappleDuration = 3f; // �׷��ø� ���� �ð�
    public float grappleCooldown = 1f; // �׷��ø� ��ٿ� �ð�
    private float grappleTimer; // �׷��ø� Ÿ�̸�
    private float cooldownTimer; // ��ٿ� Ÿ�̸�
    //public Image gauge;

    // Start is called before the first frame update
    void Start()
    {
        //gauge.fillAmount = 0f;
        line.positionCount = 2;
        line.endWidth = line.startWidth = 0.05f;
        line.SetPosition(0, transform.position);
        line.SetPosition(1, hook.position);
        line.useWorldSpace = true;
        isAttach = false;
    }

    // Update is called once per frame
    void Update()
    {
        line.SetPosition(0, transform.position);
        line.SetPosition(1, hook.position);

        if (Input.GetMouseButtonDown(0) && !isHookActive && cooldownTimer <= 0f)
        {
            hook.position = transform.position;
            mousedir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            isHookActive = true;
            isLineMax = false;
            hook.gameObject.SetActive(true);
        }

        if (isHookActive && !isLineMax && !isAttach)
        {
            hook.Translate(mousedir.normalized * Time.deltaTime * 15);

            if (Vector2.Distance(transform.position, hook.position) > 5)
            {
                isLineMax = true;
            }
        }
        else if (isHookActive && isLineMax && !isAttach)
        {
            hook.position = Vector2.MoveTowards(hook.position, transform.position, Time.deltaTime * 15);
            if (Vector2.Distance(transform.position, hook.position) < 0.1f)
            {
                isHookActive = false;
                isLineMax = false;
                hook.gameObject.SetActive(false);
            }
        }

        if (isAttach)
        {
            grappleTimer += Time.deltaTime;

            //gauge.fillAmount = 1 - (grappleTimer / grappleDuration);

            if (grappleTimer >= grappleDuration || Input.GetMouseButtonUp(0))
            {
                isAttach = false;
                isHookActive = false;
                isLineMax = false;
                hook.GetComponent<Hookg>().joint2D.enabled = false;
                hook.gameObject.SetActive(false);
                isReleased = true;
                releaseVelocity = GetComponent<Rigidbody2D>().linearVelocity;
                grappleTimer = 0f;
                cooldownTimer = grappleCooldown;

                //gauge.fillAmount = 0f;
            }
        }

        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    public Vector2 GetHookPosition()
    {
        return hook.position;
    }
}
