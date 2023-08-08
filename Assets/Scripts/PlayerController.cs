using System;
using System.Collections;
using Cinemachine;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Controller Settings")]
    public KeyCode moveLeftKey = KeyCode.LeftArrow;
    public KeyCode moveRightKey = KeyCode.RightArrow;
    public KeyCode climbUpKey = KeyCode.UpArrow;
    public KeyCode climbDownKey = KeyCode.DownArrow;
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Dependencies")]
    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private Collider2D headCollider;

    [SerializeField]
    private Collider2D feetCollider;

    [SerializeField]
    private Transform head;

    [SerializeField]
    private Transform feet;

    [SerializeField]
    private LayerMask groundMask;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Animator animator;

    [Header("Player Settings")]
    [Range(10f, 100f)]
    [SerializeField]
    private float speed = 40f;

    [Range(1f, 10f)]
    [SerializeField]
    private float jumpHeight = 4f;

    [Range(.1f, .9f)]
    [SerializeField]
    private float crouchSpeedFactor = .4f;

    [Range(.1f, .9f)]
    [SerializeField]
    private float headCastRadius = .2f;

    [Range(.1f, .9f)]
    [SerializeField]
    private float feetCastRadius = .2f;

    [Range(0f, 1f)]
    [SerializeField]
    private float deathStopCamDelay = .5f;

    private float horizontalVelocity,
        verticalVelocity,
        jumpForce;
    private bool isAlive = true;
    private bool jump = false;
    private bool crouch = false;
    private bool isCrouching = false;
    private bool climbAble = false;
    private bool isClimbing = false;
    private float defaultGScale;
    private Vector2 spawnPosition = Vector2.right * -9999;

    public Vector2 SpawnPosition
    {
        set => spawnPosition = value;
    }

    private int Horizontal()
    {
        if (Input.GetKey(moveLeftKey))
            return -1;
        if (Input.GetKey(moveRightKey))
            return 1;
        return 0;
    }

    private int Vertical()
    {
        if (Input.GetKey(climbDownKey))
            return -1;
        if (Input.GetKey(climbUpKey))
            return 1;
        return 0;
    }

    private bool CircleCastHit(Transform transform, float radius)
    {
        RaycastHit2D cast = Physics2D.CircleCast(
            transform.position,
            radius,
            Vector2.down,
            0f,
            groundMask
        );
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

    public void SetClimbAble()
    {
        climbAble = true;
    }

    public void UnsetClimbAble()
    {
        climbAble = false;
        isClimbing = false;
        rb.gravityScale = defaultGScale;
        animator.speed = 1;
    }

    public void Die()
    {
        isAlive = false;
        headCollider.enabled = feetCollider.enabled = false;
        rb.velocity = Vector2.zero;
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        CinemachineVirtualCamera vcam = GetComponentInChildren<CinemachineVirtualCamera>();
        StartCoroutine(
            WaitAndExecute(deathStopCamDelay, () => vcam.Follow = vcam.transform.parent = null)
        );
        animator.SetBool("Hurt", true);
    }

    private void Awake()
    {
        jumpForce = Mathf.Sqrt(jumpHeight * -2 * (Physics2D.gravity.y * rb.gravityScale));
        defaultGScale = rb.gravityScale;
        if (spawnPosition != Vector2.right * -9999)
            transform.position = spawnPosition;
    }

    private void Update()
    {
        if (!isAlive)
            return;
        float speedFactor = isCrouching ? crouchSpeedFactor : 1;
        horizontalVelocity = Horizontal() * speed * 10f * speedFactor;

        if (
            !spriteRenderer.flipX && horizontalVelocity < -.01
            || spriteRenderer.flipX && horizontalVelocity > .01
        )
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

        if (climbAble)
        {
            verticalVelocity = Vertical() * speed * 10f;
            if (verticalVelocity != 0 && !isClimbing)
            {
                isClimbing = true;
                rb.velocity = Vector2.zero;
                rb.gravityScale = 0;
            }
            else if (isClimbing)
                animator.speed = verticalVelocity == 0 ? 0 : 1;
            else
            {
                rb.gravityScale = defaultGScale;
                animator.speed = 1;
            }
        }

        animator.SetFloat("Speed", Mathf.Abs(horizontalVelocity));
        animator.SetBool("Grounded", IsGrounded());
        animator.SetFloat("VerticalVelocity", rb.velocity.y);
        animator.SetBool("Crouch", isCrouching);
        animator.SetBool("Climb", isClimbing);
    }

    private void FixedUpdate()
    {
        if (!isAlive)
            return;
        rb.velocity = new Vector2(horizontalVelocity * Time.fixedDeltaTime, rb.velocity.y);

        if (jump)
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jump = false;
            isClimbing = false;
        }

        headCollider.enabled = !isCrouching;

        if (isClimbing)
            rb.velocity = new Vector2(rb.velocity.x, verticalVelocity * Time.fixedDeltaTime);
    }
}
