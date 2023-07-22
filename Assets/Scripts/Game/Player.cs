using Cinemachine;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Controller Settings")]
    public KeyCode moveLeftKey = KeyCode.LeftArrow;
    public KeyCode moveRightKey = KeyCode.RightArrow;
    public KeyCode climbKey = KeyCode.UpArrow;
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Dependencies")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D headCollider;
    [SerializeField] private Collider2D feetCollider;
    [SerializeField] private Transform head;
    [SerializeField] private Transform feet;
    [SerializeField] private Animator animator;
    [SerializeField] private LayerMask groundMask;

    [Header("Player Settings")]
    [SerializeField] private float speed = 40f;
    [SerializeField] private float jumpHeight = 3f;
    [SerializeField] private float crouchSpeedFactor = .4f;

    private bool isFacingRight = true;
    private float velocity;
    private float jumpForce;
    private bool jump = false;
    private bool crouch = false;
    private bool isCrouching = false;

    private int Horizontal()
    {
        if (Input.GetKey(moveLeftKey)) return -1;
        if (Input.GetKey(moveRightKey)) return 1;
        return 0;
    }

    private void Flip()
    {
        Vector2 flipScale = transform.localScale;
        flipScale.x *= -1;
        transform.localScale = flipScale;
    }

    private bool CircleCastHit(Transform transform, float radius)
    {
        RaycastHit2D cast = Physics2D.CircleCast(transform.position, radius, Vector2.down, 0f, groundMask);
        return cast.collider != null;
    }

    private bool IsGrounded()
    {
        return CircleCastHit(feet, .2f);
    }

    private bool HeadBump()
    {
        return CircleCastHit(head, .2f);
    }

    private void Awake()
    {
        jumpForce = Mathf.Sqrt(jumpHeight * -2 * (Physics2D.gravity.y * rb.gravityScale));
    }

    private void Update()
    {
        float speedFactor = isCrouching ? crouchSpeedFactor : 1;
        velocity = Horizontal() * speed * 10f * speedFactor;

        animator.SetFloat("Speed", Mathf.Abs(velocity));

        if (isFacingRight && velocity < -.01 || !isFacingRight && velocity > .01)
        {
            isFacingRight = !isFacingRight;
            Flip();
        }

        if (Input.GetKeyDown(jumpKey) && IsGrounded())
            jump = true;

        animator.SetBool("Grounded", IsGrounded());
        animator.SetFloat("VerticalVelocity", rb.velocity.y);

        if (Input.GetKeyDown(crouchKey) && IsGrounded())
            crouch = isCrouching = true;
        else if (Input.GetKeyUp(crouchKey))
        {
            crouch = false;
            isCrouching = HeadBump();
        }
        else if (!crouch && !HeadBump())
            isCrouching = false;
    }

    private void FixedUpdate()
    {
        rb.velocity = velocity * Time.fixedDeltaTime * Vector2.right + rb.velocity.y * Vector2.up;

        if (jump)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jump = false;
        }

        headCollider.enabled = !isCrouching;
        animator.SetBool("Crouch", isCrouching);
    }
}
