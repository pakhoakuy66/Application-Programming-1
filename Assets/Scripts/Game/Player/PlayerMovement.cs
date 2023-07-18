using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float Speed;
    [SerializeField] private CharacterController2D controller;
    [SerializeField] private Animator animator;

    private bool Jump = false;
    private bool Crouch = false;
    private float HorizontalVelocity = 0f;

    private void Update()
    {
        HorizontalMovement();

        Jumping();

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

    private void Jumping()
    {
        if (Input.GetButtonDown("Jump"))
        {
            Jump = true;
            animator.SetBool("IsJumping", true);
        }
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!collision.collider.CompareTag("Player"))
            animator.SetBool("IsJumping", false);
    }
}
