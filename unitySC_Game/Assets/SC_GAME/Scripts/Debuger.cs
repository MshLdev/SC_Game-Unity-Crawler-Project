using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEBUGER : MonoBehaviour
{
    public GameObject rgdlTestObject;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F2))
        {
            GameObject RoachRgdl =  Instantiate(rgdlTestObject, new Vector3(5f, 10f, 8f), Quaternion.identity);
            RoachRgdl.GetComponent<Enemy>().SwitchRagdoll();
        } 
    }
}