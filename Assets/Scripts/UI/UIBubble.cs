using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIBubble : MonoBehaviour
{
    public RectTransform uiBubble;
    public bool isPopped;

    private void Awake()
    {
        uiBubble.localScale = Vector3.zero;
    }

    public void Initialize()
    {
        uiBubble.localScale = Vector3.zero;
        GlobalGameManager.Instance.soundManager.PlaySound(SFXType.BubblePop);
        uiBubble.DOScale(Vector3.one, UIGameplayManager.TRANSITION_TIME);
        isPopped = false;
    }

    public void Inflate()
    {
        uiBubble.localScale = Vector3.one;
        isPopped = false;
    }
    public void Pop()
    {
        uiBubble.localScale = Vector3.zero;
        isPopped = true;
    }
}
