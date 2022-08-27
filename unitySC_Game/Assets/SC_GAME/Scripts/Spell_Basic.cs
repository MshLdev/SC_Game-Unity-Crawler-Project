using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell_Basic : MonoBehaviour
{
    public AudioMenager audioM;
    public GameObject vfx;
    public float MaxLifeDuration;
    public float Damage;

    void Start()
    {
        audioM = GameObject.Find("_GAME").GetComponent<AudioMenager>();
        audioM.AudioAtPosition(0, transform.position);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Destructable")
        {
            audioM.AudioAtPosition(AudioMenager.clips.damageWood, transform.position);
            Destroy(collision.gameObject);
        }

        if(vfx)
        {
            GameObject vfxInstantion = Instantiate(vfx, transform.position, Quaternion.identity);
            audioM.AudioAtPosition(AudioMenager.clips.fireCrack, transform.position);
            Destroy(vfxInstantion, 5.0f);
        }
            
        Destroy(gameObject);
    }
}
