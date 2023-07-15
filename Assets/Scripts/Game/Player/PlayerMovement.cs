using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float Speed = 40f;
    [SerializeField] private CharacterController2D controller;

    private bool Jump = false;
    private bool Crouch = false;
    private float HorizontalAxis = 0f;

    private void Update()
    {
        HorizontalAxis = Input.GetAxisRaw("Horizontal") * Speed;

        if (Input.GetButtonDown("Jump")) Jump = true;

        if (Input.GetKey(KeyCode.LeftControl)) Crouch = true;
        else if (Input.GetKeyUp(KeyCode.LeftControl)) Crouch = false;
    }

    private void FixedUpdate()
    {
        controller.Move(HorizontalAxis * Time.fixedDeltaTime, Crouch, Jump);
        Jump = false;
    }
}
