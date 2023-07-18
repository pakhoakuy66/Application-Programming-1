using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private CharacterController2D controller;
    [SerializeField] private Animator animator;

    private bool jump = false;
    private bool crouch = false;
    private float HorizontalVelocity = 0f;

    private void Update()
    {
        HorizontalMovement();

        JumpStart();

        Crouching();
    }

    private void FixedUpdate()
    {
        controller.Move(HorizontalVelocity * Time.fixedDeltaTime, crouch, jump);
        jump = false;
    }

    private void HorizontalMovement()
    {
        HorizontalVelocity = Input.GetAxisRaw("Horizontal") * speed;
        animator.SetFloat("Speed", Mathf.Abs(HorizontalVelocity));
    }

    private void JumpStart()
    {
        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            animator.SetBool("Jump", true);
        }
    }

    public void JumpEnd() => animator.SetBool("Jump", false);
    private void Crouching()
    {
        if (Input.GetKey(KeyCode.LeftControl))
            crouch = true;
        else if (Input.GetKeyUp(KeyCode.LeftControl))
            crouch = false;
    }

    public void CrouchCheck(bool CeilHit) => animator.SetBool("Crouch", CeilHit);
}
