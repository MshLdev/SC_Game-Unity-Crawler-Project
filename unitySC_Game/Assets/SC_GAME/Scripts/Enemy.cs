using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Animator anim;
    constants cnst;
    private bool started = false;

    float statecoolfown = 0.6f;
    float statetimer = 0f;
    float Idlefactor = 1f;

    void Start()
    {
        if(!started)
        {
        anim = gameObject.GetComponent<Animator>();
        //not ised for now...
        //cnst = GameObject.Find("_GAME").GetComponent<constants>();

        //Testing color variants
        //foreach ( Renderer go in GetComponentsInChildren<Renderer>() ) 
            //go.material.SetColor("_EyeColor", new Color32((byte)Random.Range(0, 250), (byte)Random.Range(0, 250), (byte)Random.Range(0, 250), 255));
        
        SwitchRagdoll();  
        started = true;  
        }
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
        foreach ( Rigidbody rb in GetComponentsInChildren<Rigidbody>() ) 
        {
            bool currentState = rb.isKinematic;
            rb.isKinematic = !currentState;
            anim.enabled = !currentState;
        }
        

        float eyeglow = 77f;
        if(!anim.enabled)
            eyeglow = 0f;

        foreach ( Renderer go in GetComponentsInChildren<Renderer>() ) 
            go.material.SetFloat("_EyeGlow", eyeglow);
     }

     public void ForceStart()
     {
        Start();
     }
}
