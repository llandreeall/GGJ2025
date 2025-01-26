using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffect : MonoBehaviour
{
    public AudioSource audioSource;

    public void Initialize(AudioClip clip, float volume)
    {
        audioSource.clip = clip;
        audioSource.volume = volume;
        StartCoroutine(StartPlaying());
    }

    IEnumerator StartPlaying()
    {
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);
        audioSource.Stop();

    }
}
