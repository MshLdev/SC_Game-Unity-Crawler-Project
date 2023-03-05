using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class SoundRandomizer : MonoBehaviour
{
    

    void Start()
    {
        AudioSource sound = transform.GetComponent<AudioSource>();

        sound.pitch =  Random.Range(0.8f, 1.4f);
        sound.PlayDelayed(Random.Range(0f, 0.8f));
        Destroy(transform.GetComponent<SoundRandomizer>());
    }

}


