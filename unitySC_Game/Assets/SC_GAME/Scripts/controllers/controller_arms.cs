using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controller_arms : MonoBehaviour
{
    
    public float smoothFacor;
    public float multiplier;
    public float BaseRotX;

    private void Update()
    {
        if(!Cursor.visible)
        {
            float mouseX = Input.GetAxisRaw("Mouse X") * multiplier;
            float mouseY = Input.GetAxisRaw("Mouse Y") * multiplier + BaseRotX;;

            Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
            Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

            Quaternion targetRotation = rotationX * rotationY;
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smoothFacor * Time.deltaTime);
        }

    }
}
