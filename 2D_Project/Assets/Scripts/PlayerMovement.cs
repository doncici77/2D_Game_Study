using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float jumpForce = 10.0f;
    public float runSpeed = 10.0f;

    private Rigidbody2D rb;
    public bool isGrounded;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private PlayerAnimation playerAnimation;
    private float localScaleX;

    private bool isGrapple = false;
    private Vector3 point;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAnimation = GetComponent<PlayerAnimation>();
    }

    private void Update()
    {
        Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
        bool isRight = point.x > transform.position.x;
        transform.localScale = new Vector3(isRight ? 6 : -6, transform.localScale.y, transform.localScale.z);
    }

    public void HandleMovement()
    {
        float currentSpeed = moveSpeed;
        float moveInput = Input.GetAxis("Horizontal");

        HandlePlayerMove(moveInput, ref currentSpeed);
        HandlePlayerAnimation(moveInput);
    }

    private void HandlePlayerMove(float moveInput, ref float currentSpeed)
    {
        // �׷��� ���� ����
        if (Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKey(KeyCode.Mouse1))
        {
            isGrapple = true;
        }

        // �޸���
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = runSpeed;
        }
        else
        {
            currentSpeed = moveSpeed;
        }

        // �׷��� ���� �ƴ� ���� �̵� ����
        if (!isGrapple)
        {
            rb.linearVelocity = new Vector2(moveInput * currentSpeed, rb.linearVelocity.y);
        }

        // ����
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    private void HandlePlayerAnimation(float moveInput)
    {
        if (playerAnimation == null) return;

        // �ȱ� & �޸���
        playerAnimation.SetWalking(moveInput != 0);
        playerAnimation.SetRunnig(Input.GetKey(KeyCode.LeftShift));

        // ���� / ���� / ����
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            playerAnimation.SetJumping(true);
        }
        else if (!isGrounded && rb.linearVelocity.y < -0.1f)
        {
            playerAnimation.SetFalling(true);
        }
        else if (isGrounded && !Input.GetKey(KeyCode.Mouse1))
        {
            playerAnimation.PlayLanding();
            isGrapple = false;
        }

        // �׷��� ���� ���� ���� ó��
        if (isGrapple && Input.GetKey(KeyCode.Mouse1))
        {
            playerAnimation.SetFalling(true);
        }
    }
}

