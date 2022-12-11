using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Animator anim;
    constants cnst;
    //BoxCollider entityColider;
    

    float statecoolfown = 0.6f;
    float statetimer = 0f;
    float Idlefactor = 1f;

    void Awake()
    {
        //entityColider = gameObject.GetComponent<BoxCollider>();
        anim = gameObject.GetComponent<Animator>();

        //not ised for now...
        //cnst = GameObject.Find("_GAME").GetComponent<constants>();

        //Testing color variants
        //foreach ( Renderer go in GetComponentsInChildren<Renderer>() ) 
            //go.material.SetColor("_EyeColor", new Color32((byte)Random.Range(0, 250), (byte)Random.Range(0, 250), (byte)Random.Range(0, 250), 255))
        
    }

    void Update()
    {
        animationstaging();
    }

    void animationstaging()
    {
        if(statetimer > statecoolfown)
        {
            anim.SetInteger("State", (int)Random.Range(0f, 2.4f));
            Idlefactor = Random.Range(0f, 1f);
            statetimer = 0f;
        }
        else
        statetimer += Time.deltaTime;

        anim.SetFloat("blend_IDLE", Idlefactor, 0.2f, Time.deltaTime);
    }


    public void SwitchRagdoll() 
    {
        Debug.Log("Switching!");
        //entityColider.enabled = !entityColider.enabled;
        anim.enabled = !anim.enabled;

        foreach ( Rigidbody rb in transform.GetComponentsInChildren<Rigidbody>() ) 
            rb.isKinematic = !rb.isKinematic;
        
        
        float eyeglow = 77f;
        if(!anim.enabled)
            eyeglow = 0f;

        foreach ( Renderer go in GetComponentsInChildren<Renderer>() ) 
            go.material.SetFloat("_EyeGlow", eyeglow);
     }
}
