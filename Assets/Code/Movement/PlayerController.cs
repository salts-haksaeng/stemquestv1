using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 7f;
    [SerializeField] float jumpForce = 15f;
    [SerializeField] float checkRadius = 0.3f;
    [SerializeField] float jumpTime = 0.1f;

    public LayerMask groundLayer;
    public Transform groundCheck;

    float jumpHoldTime;
    float xInput;

    bool facingRight;
    bool jump;
    bool isJumping;
    bool isGrounded;
    bool PreviouslyGrounded;

    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start() {
        facingRight = true;
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(xInput * moveSpeed, rb.velocity.y, 0f);

        //flipping the player
        if(xInput < 0 && facingRight)
        {
            flipPlayer();
        }
        else if(xInput > 0 && !facingRight)
        {
            flipPlayer();
        }

        isGrounded = Physics.CheckSphere(groundCheck.position, checkRadius, groundLayer);

        if(!jump)
        {
            PreviouslyGrounded = false;
            isJumping = false;
            return;
        }

        if(isGrounded && jump && !isJumping && !PreviouslyGrounded)
        {
            PreviouslyGrounded = true;
            isJumping = true;
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, 0f);
            jumpHoldTime = jumpTime;
        }
        else if(isJumping && jump)
        {
            if(jumpHoldTime > 0)
            {
                rb.velocity = new Vector3(rb.velocity.x, jumpForce, 0f);
                jumpHoldTime -= Time.fixedDeltaTime;
            }
            else
                isJumping = false;
        }
    }

    void flipPlayer()
    {
        facingRight = !facingRight;

        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    public void HorizontalInput(float value)
    {
        xInput = value;
    }

    public void JumpInput(bool value)
    {
        jump = value;
    }
}
