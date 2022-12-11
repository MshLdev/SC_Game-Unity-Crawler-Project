using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class controller_player : MonoBehaviour
{
    public float walkingSpeed       = 7.5f;
    public float runningSpeed       = 11.5f;
    public float crouchingSpeed     = 3.5f;

    public float jumpSpeed          = 8.0f;
    public float gravity            = 20.0f;
    public float lookSpeed          = 2.0f;
    public float lookXLimit         = 45.0f;

    CharacterController characterController;
    public Camera playerCamera;

    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (Cursor.visible)
            return;
            
        movement();
        rotation();
    }


    void movement()
    {
         // We are grounded, so recalculate move direction based on axes
        // Calculate Movement
        float speed = checkSpeed();
        float movementDirectionY = moveDirection.y;
        moveDirection = (transform.forward * Input.GetAxis("Vertical")) + (transform.right * Input.GetAxis("Horizontal"));
        moveDirection.Normalize();
        moveDirection *= speed;

        //Consider Jumping
        if (Input.GetButton("Jump")  && characterController.isGrounded)
            moveDirection.y = jumpSpeed;
        //nie rozumim tej linijki
        else
            moveDirection.y = movementDirectionY;
        
        // Apply gravity. Gravity is multiplied by deltaTime twice,because gravity should be applied as an acceleration (ms^-2)
        if (!characterController.isGrounded)
            moveDirection.y -= gravity * Time.deltaTime;
        

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);
    }
    

    void rotation()
    {
        rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
    }

    float checkSpeed()
    {
        if (Input.GetKey(KeyCode.LeftShift))    
            return runningSpeed;

        if (Input.GetKey(KeyCode.LeftControl))   
            return crouchingSpeed;

        else
            return walkingSpeed;
    }
}