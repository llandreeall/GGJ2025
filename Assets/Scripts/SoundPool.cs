using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SFXType
{
    SubmarineCrash,
    SubmarineRecover,
    PlayerJump,
    PlayerStartFix,
    PlayerAddProgress,
    PlayerHit,
    PlayerTakeBubbles,
    BubblePop,
    UI_Bttn,
    UI_Win,
    UI_Lose,
    UI_Popup
}

[System.Serializable]
public struct SoundStruct
{
    public AudioClip clip;
    public SFXType sfxType;
    public float volume;
}

public class SoundPool : ObjectPool<SoundEffect>
{
    public SoundPool(SoundEffect sfxPrefab, Transform parentTransform, int startSize = 1) : base(sfxPrefab, parentTransform, startSize)
    {

    }
}