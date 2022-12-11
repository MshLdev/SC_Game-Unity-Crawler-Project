using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell_Basic : MonoBehaviour
{
    public AudioMenager audioM;
    public GameObject vfx;

    public float MaxLifeDuration;
    public float Damage;
    public float DamageRange;
    public float DamageForce;

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

            Vector3 explosionPos = transform.position;
            Collider[] colliders = Physics.OverlapSphere(explosionPos, DamageRange);

            foreach (Collider hit in colliders)
            {
                if(hit.gameObject.tag == "Spell")
                    break;
                
                if(hit.gameObject.tag == "Enemy")
                    hit.transform.root.GetComponent<Enemy>().SwitchRagdoll();
                    
                Rigidbody rb = hit.GetComponent<Rigidbody>();
                if (rb != null)
                    rb.AddExplosionForce(DamageForce, explosionPos, DamageRange, 3.0F);
            }
        }
            
        Destroy(gameObject);
    }
}
