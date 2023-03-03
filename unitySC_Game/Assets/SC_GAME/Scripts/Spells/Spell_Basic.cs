using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell_Basic : MonoBehaviour
{
    public AudioMenager audioM;
    public GameObject vfx;


    public float        Damage;
    public bool         explosion;
    public bool         explosionDamage;
    public float        explosionRadius;
    public float        explosionForce;

    void Start()
    {
        audioM = GameObject.Find("_GAME").GetComponent<AudioMenager>();
        audioM.AudioAtPosition(0, transform.position);
    }

    void OnCollisionEnter(Collision collision)
    {
        if(explosion)
            explode();
        switch (collision.gameObject.tag)
        {
        case "Destructable":
            hit();
            audioM.AudioAtPosition(AudioMenager.clips.damageWood, transform.position);   
            Destroy(collision.gameObject);
            break;
        case "Enemy":
            hit();
            EnemyDamage(collision.gameObject, 1f);
            break;
        default:
            hit();
            break;
        }
    }

    void hit()
    {
        GameObject vfxInstantion = Instantiate(vfx, transform.position, Quaternion.identity);
        audioM.AudioAtPosition(AudioMenager.clips.fireCrack, transform.position);

        Destroy(vfxInstantion, 5.0f);      
        Destroy(gameObject);
    }

    void explode()
    {
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, explosionRadius);

        foreach (Collider hit in colliders)
        {
            //Add Force
            if(hit.gameObject.layer == 7) //7 stands for IK, aka dead mobs
            {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
                rb.AddExplosionForce(explosionForce, explosionPos, explosionRadius, 3.0F);
            }
            //add explosion damage to the enemies
            if(hit.gameObject.tag == "Enemy" && explosionDamage)
                EnemyDamage(hit.gameObject, 0.35f);
        }

    }

    void EnemyDamage(GameObject enemy, float damagescale)
    {
        enemy.GetComponent<Enemy>().DealDamage(Damage * damagescale);
    }
}
