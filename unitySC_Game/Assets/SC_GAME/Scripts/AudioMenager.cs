using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMenager : MonoBehaviour
{
    public List<AudioClip> audioClips;
    public GameObject AudioPrefab;
    public GameObject player;


    public enum clips
    {
      fireBurst = 0,
      fireCrack = 1,
      fireTorch = 2,
      ///////////////////
      damageWood = 3,
      lvSwitch = 4,
      ///////////////////
      ui_select = 5,
      ui_close = 6, 
      ui_hover = 7,
    };

    void Start() 
    {
      audioClips.Add(Resources.Load<AudioClip>("Audio/fireBurst"));
      audioClips.Add(Resources.Load<AudioClip>("Audio/fireCrack"));
      audioClips.Add(Resources.Load<AudioClip>("Audio/fireTorch"));
      audioClips.Add(Resources.Load<AudioClip>("Audio/damageWood"));
      audioClips.Add(Resources.Load<AudioClip>("Audio/lvSwitch"));
      audioClips.Add(Resources.Load<AudioClip>("Audio/ui_select"));
      audioClips.Add(Resources.Load<AudioClip>("Audio/ui_close"));
      audioClips.Add(Resources.Load<AudioClip>("Audio/ui_hover"));
    }

    public void AudioAtPosition(clips audioId, Vector3 position)
    {
       GameObject audioInstance = Instantiate(AudioPrefab, position, Quaternion.identity);

       audioInstance.GetComponent<AudioSource>().clip = audioClips[(int)audioId];
       audioInstance.GetComponent<AudioSource>().Play();
       Destroy(audioInstance, 5f);
    }

    public void AudioAsChild(clips audioId, Vector3 position, Transform parentGO)
    {
       GameObject audioInstance = Instantiate(AudioPrefab, position, Quaternion.identity);
       audioInstance.transform.SetParent(parentGO);

       audioInstance.GetComponent<AudioSource>().clip = audioClips[(int)audioId];
       audioInstance.GetComponent<AudioSource>().Play();
       Destroy(audioInstance, 5f);
    }

    public void AudioAtPlayer(clips audioId)
    {
      AudioAtPosition(audioId, player.transform.position);
    }
}
