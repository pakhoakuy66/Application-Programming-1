using System;
using System.Collections;
using Cinemachine;
using UnityEngine;

public class PlayerController : MonoBehaviour
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
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Optional Components")]
    [SerializeField] private Animator animator;
    public Transform spawnPoint;

    [Header("Player Settings")]
    [Range(10f, 100f)] [SerializeField] private float speed = 40f;
    [Range(1f, 10f)] [SerializeField] private float jumpHeight = 4f;
    [Range(.1f, .9f)] [SerializeField] private float crouchSpeedFactor = .4f;
    [Range(.1f, .9f)] [SerializeField] private float headCastRadius = .2f;
    [Range(.1f, .9f)] [SerializeField] private float feetCastRadius = .2f;
    [Range(0f, 1f)] [SerializeField] private float deathStopCamDelay = .5f;

    private float velocity, jumpForce;
    private bool isAlive = true;
    private bool jump = false;
    private bool crouch = false;
    private bool isCrouching = false;

    private int Horizontal()
    {
        if (Input.GetKey(moveLeftKey)) return -1;
        if (Input.GetKey(moveRightKey)) return 1;
        return 0;
    }

    private bool CircleCastHit(Transform transform, float radius)
    {
        RaycastHit2D cast = Physics2D.CircleCast(transform.position, radius, Vector2.down, 0f, groundMask);
        return cast.collider != null;
    }

    private bool IsGrounded()
    {
        return CircleCastHit(feet, feetCastRadius);
    }

    private bool HeadBump()
    {
        return CircleCastHit(head, headCastRadius);
    }

    private IEnumerator WaitAndExecute(float waitTime, Action callBack)
    {
        yield return new WaitForSeconds(waitTime);
        callBack.Invoke();
    }

    public void Die()
    {
        isAlive = false;
        headCollider.enabled = feetCollider.enabled = false;
        rb.velocity = Vector2.zero;
        rb.AddForce(jumpForce * Vector2.up, ForceMode2D.Impulse);
        CinemachineVirtualCamera vcam = GetComponentInChildren<CinemachineVirtualCamera>();
        StartCoroutine(WaitAndExecute(deathStopCamDelay, () => vcam.Follow = vcam.transform.parent = null));
        if (animator != null)
        {
            animator.SetFloat("VerticalVelocity", 0);
            animator.SetBool("Hurt", true);
        }
    }

    private void Awake()
    {
        jumpForce = Mathf.Sqrt(jumpHeight * -2 * (Physics2D.gravity.y * rb.gravityScale));
        if(spawnPoint != null) transform.position = spawnPoint.position;
    }

    private void Update()
    {
        if (!isAlive) return;
        float speedFactor = isCrouching ? crouchSpeedFactor : 1;
        velocity = Horizontal() * speed * 10f * speedFactor;

        if (!spriteRenderer.flipX && velocity < -.01 || spriteRenderer.flipX && velocity > .01)
            spriteRenderer.flipX = !spriteRenderer.flipX;

        if (Input.GetKeyDown(jumpKey) && IsGrounded())
            jump = true;

        if (Input.GetKey(crouchKey))
        {
            crouch = true;
            isCrouching = IsGrounded();
        }
        else if (Input.GetKeyUp(crouchKey))
        {
            crouch = false;
            isCrouching = HeadBump();
        }
        else if (!crouch && !HeadBump())
            isCrouching = false;

        if(animator != null)
        {
            animator.SetFloat("Speed", Mathf.Abs(velocity));
            animator.SetBool("Grounded", IsGrounded());
            animator.SetFloat("VerticalVelocity", rb.velocity.y);
            animator.SetBool("Crouch", isCrouching);
        }
    }

    private void FixedUpdate()
    {
        if (!isAlive) return;
        rb.velocity = new Vector2(velocity * Time.fixedDeltaTime, rb.velocity.y);

        if (jump)
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jump = false;
        }

        headCollider.enabled = !isCrouching;
    }
}
