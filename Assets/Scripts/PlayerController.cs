using System;
using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Controller Settings")]
    public KeyCode moveLeftKey = KeyCode.LeftArrow;
    public KeyCode moveRightKey = KeyCode.RightArrow;
    public KeyCode climbUpKey = KeyCode.UpArrow;
    public KeyCode climbDownKey = KeyCode.DownArrow;
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode crouchKey = KeyCode.LeftControl;
    public KeyCode dashKey = KeyCode.LeftShift;

    public AudioSource audioSource;
    public AudioClip death;

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

    [SerializeField]
    private TrailRenderer trailRenderer;

    [Header("Player Settings")]
    [SerializeField]
    private float speed = 40f;

    [SerializeField]
    private float dashSpeed = 50f;

    [SerializeField]
    private float jumpHeight = 4f;

    [SerializeField]
    private float crouchSpeedFactor = .4f;

    [SerializeField]
    private float headCastRadius = .2f;

    [SerializeField]
    private float feetCastRadius = .2f;

    [SerializeField]
    private float deathStopCamDelay = .5f;

    [SerializeField]
    private int jumpCount = 1;

    [SerializeField]
    private bool dashUnlocked = false;

    [SerializeField]
    private float dashCoolDown = 2f;

    [SerializeField]
    private float dashMaxTime = 1f;

    private int jumpAbleCount;
    private int direction = 1;
    private float horizontalVelocity,
        verticalVelocity,
        jumpForce;
    private float dashTimer = 0;

    private bool isAlive = true;
    private bool dashAble = true;
    private bool jump = false;
    private bool isJumping = false;
    private bool crouch = false;
    private bool isCrouching = false;
    private bool climbAble = false;
    private bool isClimbing = false;
    private bool isDashing = false;
    private bool isDashCooldown = false;
    private float defaultGScale;
    public GameObject gameover;

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

    private bool IsFalling()
    {
        return !IsGrounded() && !isJumping;
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
        audioSource.Stop();
        audioSource.PlayOneShot(death);
        isAlive = false;
        horizontalVelocity = 0;
        isCrouching = true;
        headCollider.enabled = feetCollider.enabled = false;
        rb.velocity = Vector2.zero;
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        CinemachineVirtualCamera vcam = GetComponentInChildren<CinemachineVirtualCamera>();
        StartCoroutine(
            WaitAndExecute(
                deathStopCamDelay,
                () =>
                {
                    vcam.Follow = vcam.transform.parent = null;
                    StartCoroutine(
                        WaitAndExecute(
                            deathStopCamDelay,
                            () =>
                            {
                                if (gameover != null)
                                    gameover.SetActive(true);
                                else
                                    SceneManager.LoadScene(
                                        SceneManager.GetActiveScene().buildIndex
                                    );
                            }
                        )
                    );
                }
            )
        );
        animator.SetBool("Hurt", true);
    }

    private void Awake()
    {
        jumpForce = Mathf.Sqrt(jumpHeight * -2 * (Physics2D.gravity.y * rb.gravityScale));
        defaultGScale = rb.gravityScale;
        jumpAbleCount = jumpCount;
    }

    private void Update()
    {
        if (!isAlive)
            return;
        if (isDashing)
            return;
        float speedFactor = isCrouching ? crouchSpeedFactor : 1;
        horizontalVelocity = Horizontal() * speed * 10f * speedFactor;

        if (
            !spriteRenderer.flipX && horizontalVelocity < -.01
            || spriteRenderer.flipX && horizontalVelocity > .01
        )
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
            direction = spriteRenderer.flipX ? -1 : 1;
        }

        if (Input.GetKeyDown(jumpKey) && jumpAbleCount > 0 && (!IsFalling() || isClimbing))
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

        if (IsGrounded() && rb.velocity.y <= .01f || isClimbing)
        {
            jumpAbleCount = jumpCount;
            isJumping = false;
        }

        if (dashUnlocked && Input.GetKeyDown(dashKey) && dashAble && !isClimbing && !isCrouching)
        {
            rb.gravityScale = 0;
            horizontalVelocity = 0;
            isDashing = trailRenderer.emitting = true;
            dashAble = false;
        }

        if (isDashCooldown)
            dashTimer -= Time.deltaTime;

        if (isDashCooldown && dashTimer <= 0)
        {
            isDashCooldown = false;
            dashTimer = 0;
            dashAble = true;
        }

        animator.SetFloat("Speed", Mathf.Abs(horizontalVelocity));
        animator.SetBool("Jump", (isJumping || IsFalling()) && !isDashing);
        animator.SetFloat("VerticalVelocity", rb.velocity.y);
        animator.SetBool("Crouch", isCrouching);
        animator.SetBool("Climb", isClimbing);
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontalVelocity * Time.fixedDeltaTime, rb.velocity.y);

        if (jump)
        {
            jumpAbleCount--;
            rb.velocity = Vector2.zero;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jump = false;
            isJumping = true;
            isClimbing = false;
        }

        headCollider.enabled = !isCrouching;

        if (isClimbing)
            rb.velocity = new Vector2(rb.velocity.x, verticalVelocity * Time.fixedDeltaTime);

        if (isDashing)
        {
            dashTimer += Time.fixedDeltaTime;
            rb.velocity = new Vector2(direction * Time.fixedDeltaTime * dashSpeed * 10f, 0);
            if (dashTimer >= dashMaxTime)
            {
                rb.gravityScale = defaultGScale;
                dashTimer = dashCoolDown;
                isDashCooldown = true;
                isDashing = trailRenderer.emitting = false;
            }
        }
    }
}
