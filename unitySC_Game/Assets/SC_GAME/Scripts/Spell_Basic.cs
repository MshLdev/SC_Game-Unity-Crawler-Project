using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell_Basic : MonoBehaviour
{
    public AudioMenager audiomenager;
    public GameObject vfx;
    public float MaxLifeDuration;
    public float Damage;

    void Start()
    {
        audiomenager = GameObject.Find("_GAME").GetComponent<AudioMenager>();
        audiomenager.AudioAtPosition(0, transform.position);
    }

    void OnCollisionEnter(Collision collision)
    {
        
        foreach (ContactPoint contact in collision.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.white);
        }

        if(vfx)
        {
            GameObject vfxInstantion = Instantiate(vfx, transform.position, Quaternion.identity);
            audiomenager.AudioAtPosition(1, transform.position);
            Destroy(vfxInstantion, 5.0f);
        }
            
        Destroy(gameObject);
        //if (collision.relativeVelocity.magnitude > 2)
    }
}
