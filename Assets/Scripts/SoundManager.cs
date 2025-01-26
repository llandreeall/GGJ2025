using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public SoundPool sfxPool;
    public AudioSource musicPlayer;
    [SerializeField] private float musicVolume;
    [SerializeField] private AudioClip musicClip;
    [SerializeField] private int poolSize = 1;
    [SerializeField] private SoundEffect sfxPrefab;
    [SerializeField] private Transform sfxParent;
    [SerializeField] private List<SoundStruct> sfxList;

    private void Awake()
    {
        sfxPool = new SoundPool(sfxPrefab, sfxParent, poolSize);
        musicPlayer.clip = musicClip;
        musicPlayer.volume = musicVolume;
        musicPlayer.Play();
    }

    public void PlaySound(SFXType sfxType)
    {
        var obj = sfxPool.GetRandomAvailable();
        obj.Initialize(sfxList[(int)sfxType].clip, sfxList[(int)sfxType].volume);
    }
}
