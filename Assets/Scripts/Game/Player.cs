using System.Collections;
using System.Collections.Generic;
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

    private bool IsCollide(Transform transform, LayerMask mask)
    {
        RaycastHit2D cast = Physics2D.CircleCast(transform.position, .3f, Vector2.down, 0f, mask);
        return cast.collider != null;
    }

    private void Awake()
    {
        jumpForce = Mathf.Sqrt(jumpHeight * -2 * (Physics2D.gravity.y * rb.gravityScale));
    }

    private void Update()
    {
        velocity = Horizontal() * speed;
        animator.SetFloat("Speed", Mathf.Abs(velocity));
        if (isFacingRight && velocity < -.01 || !isFacingRight && velocity > .01)
        {
            isFacingRight = !isFacingRight;
            Flip();
        }

        if (Input.GetKeyDown(jumpKey) && IsCollide(feet, groundMask))
            jump = true;

        if (Input.GetKeyDown(crouchKey))
            crouch = true;
        else if (Input.GetKeyUp(crouchKey))
            crouch = false;

        animator.SetBool("Jump", !IsCollide(feet, groundMask));
    }

    private void FixedUpdate()
    {
        Vector2 targetVelocity = 10f * velocity * Time.fixedDeltaTime * Vector2.right + Vector2.up * rb.velocity.y;
        targetVelocity.x *= crouch? crouchSpeedFactor: 1;
        rb.velocity = targetVelocity;

        if (jump)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jump = false;
        }

        headCollider.enabled = !crouch;
        animator.SetBool("Crouch", crouch);
    }
}
