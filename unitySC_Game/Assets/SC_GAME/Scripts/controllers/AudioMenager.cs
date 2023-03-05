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
      NONE        =-1,
      ///////////////////
      fireBurst   = 0,
      fireCrack   = 1,
      fireTorch   = 2,
      ///////////////////
      damageWood  = 3,
      lvSwitch    = 4,
      ///////////////////
      ui_select   = 5,
      ui_close    = 6, 
      ui_hover    = 7,
      ui_splash   = 8,
      ui_squish   = 9,
      ui_hitmark  = 10,
      Score       = 11,
    };

    public void LoadResources() 
    {
      audioClips.Add(Resources.Load<AudioClip>("Audio/fireBurst"));
      audioClips.Add(Resources.Load<AudioClip>("Audio/fireCrack"));
      audioClips.Add(Resources.Load<AudioClip>("Audio/fireTorch"));
      audioClips.Add(Resources.Load<AudioClip>("Audio/damageWood"));
      audioClips.Add(Resources.Load<AudioClip>("Audio/lvSwitch"));
      audioClips.Add(Resources.Load<AudioClip>("Audio/ui_select"));
      audioClips.Add(Resources.Load<AudioClip>("Audio/ui_close"));
      audioClips.Add(Resources.Load<AudioClip>("Audio/ui_hover"));
      audioClips.Add(Resources.Load<AudioClip>("Audio/ui_splash"));
      audioClips.Add(Resources.Load<AudioClip>("Audio/ui_squish"));
      audioClips.Add(Resources.Load<AudioClip>("Audio/ui_hitmark"));
      audioClips.Add(Resources.Load<AudioClip>("Audio/Score"));
    }

    public void AudioAtPosition(clips audioId, Vector3 position, float vol = 1f)
    {
      if(audioId == clips.NONE)
        return;

       GameObject audioInstance = Instantiate(AudioPrefab, position, Quaternion.identity);

       audioInstance.GetComponent<AudioSource>().clip = audioClips[(int)audioId];
       audioInstance.GetComponent<AudioSource>().volume = vol;
       audioInstance.GetComponent<AudioSource>().Play();
       Destroy(audioInstance, 5f);
    }

    public void AudioAsChild(clips audioId, Vector3 position, Transform parentGO)
    {
      if(audioId == clips.NONE)
        return;

       GameObject audioInstance = Instantiate(AudioPrefab, position, Quaternion.identity);
       audioInstance.transform.SetParent(parentGO);

       audioInstance.GetComponent<AudioSource>().clip = audioClips[(int)audioId];
       audioInstance.GetComponent<AudioSource>().Play();
       Destroy(audioInstance, 5f);
    }

    public void AudioAtPlayer(clips audioId, float Volume = 1f)
    {
      AudioAtPosition(audioId, player.transform.position, Volume);
    }

}
