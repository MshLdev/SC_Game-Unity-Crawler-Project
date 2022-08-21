using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMenager : MonoBehaviour
{
    public List<AudioClip> audioClips;
    public GameObject AudioPrefab;
    public GameObject player;

    public void AudioAtPosition(int audioId, Vector3 position)
    {
       GameObject audioInstance = Instantiate(AudioPrefab, position, Quaternion.identity);

       audioInstance.GetComponent<AudioSource>().clip = audioClips[audioId];
       audioInstance.GetComponent<AudioSource>().Play();
       Destroy(audioInstance, 5f);
    }

    public void AudioAsChild(int audioId, Vector3 position, Transform parentGO)
    {
       GameObject audioInstance = Instantiate(AudioPrefab, position, Quaternion.identity);
       audioInstance.transform.SetParent(parentGO);

       audioInstance.GetComponent<AudioSource>().clip = audioClips[audioId];
       audioInstance.GetComponent<AudioSource>().Play();
       Destroy(audioInstance, 5f);
    }

    public void AudioAtPlayer(int audioId)
    {
      AudioAtPosition(audioId, player.transform.position);
    }
}
