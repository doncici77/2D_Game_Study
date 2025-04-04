using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float jumpForce = 10.0f;
    public float runSpeed = 10.0f;

    private Rigidbody2D rb;
    private bool isGrounded;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private PlayerAnimation playerAnimation;
    private float localScaleX;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAnimation = GetComponent<PlayerAnimation>();
    }

    public void HandleMovement()
    {
        float currentSpeed = moveSpeed;
        float moveInput = Input.GetAxis("Horizontal");

        if (Input.GetKey(KeyCode.LeftShift))
        {
            playerAnimation.SetRunnig(true);
            currentSpeed = runSpeed;
        }
        else
        {
            playerAnimation.SetRunnig(false);
            currentSpeed = moveSpeed;
        }

        rb.linearVelocity = new Vector2(moveInput * currentSpeed, rb.linearVelocity.y);

        if (playerAnimation != null)
        {
            playerAnimation.SetWalking(moveInput != 0);
        }

        if(moveInput != 0)
        {
            if(moveInput < 0)
            {
                transform.localScale = new Vector3(-6, transform.localScale.y, transform.localScale.z);
            }
            else
            {
                transform.localScale = new Vector3(6, transform.localScale.y, transform.localScale.z);
            }
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        if (Input.GetButtonDown("Jump") && isGrounded) //점프애니메이션
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            playerAnimation.SetJumping(true);   
            Debug.Log("점프시작");
        }

        else if (!isGrounded && rb.linearVelocity.y < -0.1f) //낙하상태
        {
            playerAnimation?.SetFalling(true);
        }

        else if (isGrounded) //착지상태
        {
            playerAnimation?.PlayLanding();
        }
    }
}

