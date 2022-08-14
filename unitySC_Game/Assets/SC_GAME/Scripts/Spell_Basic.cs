using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell_Basic : MonoBehaviour
{
    //public SpellMenager spellmenager;
    public AudioMenager audiomenager;
    public GameObject vfx;
    public float MaxLifeDuration;
    public float Damage;

    void Start()
    {
        //spellmenager = GameObject.Find("_GAME").GetComponent<SpellMenager>();
        audiomenager = GameObject.Find("_GAME").GetComponent<AudioMenager>();
        audiomenager.AudioAtPosition(0, transform.position);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Destructable")
        {
            audiomenager.AudioAtPosition(2, transform.position);
            Destroy(collision.gameObject);
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
