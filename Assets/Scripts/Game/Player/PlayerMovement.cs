using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float Speed = 40f;
    [SerializeField] private CharacterController2D controller;
    [SerializeField] private Animator animator;

    private bool Jump = false;
    private bool Crouch = false;
    private float HorizontalVelocity = 0f;

    private void Update()
    {
        HorizontalMovement();

        JumpStart();

        Crouching();
    }

    private void FixedUpdate()
    {
        controller.Move(HorizontalVelocity * Time.fixedDeltaTime, Crouch, Jump);
        Jump = false;
    }

    private void HorizontalMovement()
    {
        HorizontalVelocity = Input.GetAxisRaw("Horizontal") * Speed;
        animator.SetFloat("Speed", Mathf.Abs(HorizontalVelocity));
    }

    private void JumpStart()
    {
        if (Input.GetButtonDown("Jump")) Jump = true;
    }

    private void Crouching()
    {
        if (Input.GetKey(KeyCode.LeftControl))
            Crouch = true;
        else if (Input.GetKeyUp(KeyCode.LeftControl))
            Crouch = false;
    }

    public void CrouchCheck(bool CeilHit)
    {
        animator.SetBool("IsCrouching", CeilHit);
    }
}
