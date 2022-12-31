    ///     DEPENDENCIES
    //      we will try to keep this script dependent freee
    //      and use it's public functions instead, so far this script is accessed By following Scripts:
    //
    //  -   DATABASE.cs
    //  --


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controller_arms : MonoBehaviour
{
    //Array of prefabs, TO REFACTOR LATER
    public GameObject[] tmpPRFB;
    //Sway
    public float swaySmoothnes;
    public float swayStrenght;
    public float swayBaseX;
    //Move
    Vector3 startingPosition;

    //Reference for Animators
    Animator    hand_LEFT;
    Animator    hand_RIGHT;
    
    //Transforms for Spawning Items
    Transform   wep_LEFT;
    Transform   Wep_RIGHT;

    //References for the spawned Itmes(To delete them when necesarry)(MAYBE JUST DELETE ALL CHILDREN INSTEAD??)
    GameObject  pref_LEFT;
    GameObject  pref_RIGHT;

    bool isHolding_LEFT;    //do we have consumable in hand??
    bool isHolding_Right;   //Do we have weapon in hand??
    
    void Start()
    {
        startingPosition = transform.localPosition;

        hand_LEFT   =   GameObject.Find("ARMS").transform.GetChild(1).GetComponent<Animator>();
        hand_RIGHT  =   GameObject.Find("ARMS").transform.GetChild(0).GetComponent<Animator>();

        wep_LEFT    =   GameObject.Find("WEAPON_LEFT").transform;
        Wep_RIGHT   =   GameObject.Find("WEAPON_RIGHT").transform;
    }

    private void Update()
    {
        ArmsSway();
    }

    void ArmsSway()
    {
        if(!Cursor.visible)
        {   
            //Rotation
            float mouseX = Input.GetAxisRaw("Mouse X") * swayStrenght;
            float mouseY = Input.GetAxisRaw("Mouse Y") * swayStrenght + swayBaseX;

            Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
            Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

            Quaternion targetRotation = rotationX * rotationY;
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, swaySmoothnes * Time.deltaTime);

            ///position?
            float verticalInput = Input.GetAxis("Vertical") * -0.0025f;
            float horizontalInput = Input.GetAxis("Horizontal") * -0.0025f;
            ///JUMP NOT SMOOTH!!!!
            float jumpForce = 0f;
            if(Input.GetKeyDown(KeyCode.Space))
                jumpForce = -0.025f;
            Vector3 targetPosition = startingPosition + new Vector3(horizontalInput, jumpForce, verticalInput);

            // Use the input values to move the game object
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * 10f);
        }
    }

    public void selectPotion(int idPotion)
    {
        if(pref_RIGHT)
            Destroy(pref_RIGHT);
            
        hand_RIGHT.Play("Select_Potion", -1, 0);
        pref_RIGHT = Instantiate(tmpPRFB[idPotion], Wep_RIGHT.position, Quaternion.identity, Wep_RIGHT.transform);
        pref_RIGHT.transform.localPosition = new Vector3(0, 0, 0);
        pref_RIGHT.transform.localRotation = Quaternion.Euler(new Vector3(0,0,0));
    }

    public void Select_Null()
    {
        if(pref_RIGHT)
            Destroy(pref_RIGHT);
        hand_RIGHT.Play("Idle_Magic0", -1, 0);
    }

    public void Select_drink()
    {
        hand_RIGHT.Play("Use_Potion");
    }

    public void trigger_LastItem()
    {
        hand_RIGHT.SetTrigger("LastItem");
    }
}
