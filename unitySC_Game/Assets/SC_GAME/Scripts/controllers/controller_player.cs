using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(CharacterController))]
public class controller_player : MonoBehaviour
{
    public float speedWalk          = 7.5f;         //how fast we walk
    public float speedRun           = 11.5f;        //how fast we run
    public float speedCrouch        = 3.5f;         //how fast we walk wile crouched
    public float speedJump          = 8.0f;         //how strong is the jump
    public float speedCam           = 2.0f;         //how fast is the camera

    public float gravity           = 20.0f;         //how strong is the gravity for the player
    public float camXlimit         = 45.0f;         //how for can the camera move in the X-Axis
    private float rotationX        = 0;             //Hands rotation in X-Axis (sway movement)
    private Vector3 moveDirection  = Vector3.zero;  //Vector used to store the Player movement Vector

    [SerializeField]
    private CharacterController characterController;//yes

    ///Scripts Dependencies------------------
    ///
    /// - NONE

    ///Events -------------------------------
    //
    private float LocationTimer   = 0.3f;                     //How Often we Update Players Location
    private float locationClock         = 0f;                       //Actual Timer
    public static event System.Action<Vector3>  PlayerLocation;     //Player Location Update Event

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
            moveDirection.y = speedJump;
        //nie rozumim tej linijki
        else
            moveDirection.y = movementDirectionY;
        
        // Apply gravity. Gravity is multiplied by deltaTime twice,because gravity should be applied as an acceleration (ms^-2)
        if (!characterController.isGrounded)
            moveDirection.y -= gravity * Time.deltaTime;
        

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);
        locationEvent();
    }
    

    void rotation()
    {
        rotationX += -Input.GetAxis("Mouse Y") * speedCam * Time.deltaTime * 100f;
        rotationX = Mathf.Clamp(rotationX, -camXlimit, camXlimit);
        Camera.main.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * speedCam, 0);
    }


    float checkSpeed()
    {
        if (Input.GetKey(KeyCode.LeftShift))    
            return speedRun;

        if (Input.GetKey(KeyCode.LeftControl))   
            return speedCrouch;

        else
            return speedWalk;
    }

    void locationEvent()
    {
        locationClock += Time.deltaTime;
        if(locationClock >= LocationTimer)
            {
                locationClock = 0f;
                if(PlayerLocation != null)
                    PlayerLocation?.Invoke(transform.position - new Vector3(0f, 0.4f, 0f));
            }
    }
}