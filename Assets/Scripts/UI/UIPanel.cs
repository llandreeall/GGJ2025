using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UI;

public class UIPanel : MonoBehaviour
{
    public Action onPanelOpen = null;
    public Action onPanelClosed = null;
    public bool inAnimation = false;

    [SerializeField]
    private Button closeBttn;
    [SerializeField]
    private Transform window = null;
    [SerializeField]
    private GameObject objParent = null;

    private void Awake()
    {
        ResetPanel();
        if (closeBttn != null)
        {
            closeBttn.onClick.RemoveAllListeners();
            closeBttn.onClick.AddListener(() =>
            {
                GlobalGameManager.Instance.soundManager.PlaySound(SFXType.UI_Bttn);
                HidePanel();
            });
        }
    }

    public void ShowPanel()
    {
        GlobalGameManager.Instance.soundManager.PlaySound(SFXType.UI_Popup);
        inAnimation = true;
        objParent.SetActive(true);
        Sequence seq = DOTween.Sequence();
        seq.Append(window.DOScale(Vector3.one * 1.1f, UIGameplayManager.OVERFLOW_TIME));
        seq.Append(window.DOScale(Vector3.one * 0.9f, UIGameplayManager.SHRINK_TIME));
        seq.Append(window.DOScale(Vector3.one, UIGameplayManager.GROWBACK_TIME)).OnComplete(() => {
            if (onPanelOpen != null)
            {
                onPanelOpen.Invoke();
            }
            inAnimation = false;
        });
    }

    public void HidePanel()
    {
        GlobalGameManager.Instance.soundManager.PlaySound(SFXType.UI_Popup);
        inAnimation = true;
        objParent.SetActive(true);
        window.DOScale(Vector3.zero, UIGameplayManager.SHRINK_TIME).OnComplete(() => {
            ResetPanel();
            if (onPanelClosed != null)
            {
                onPanelClosed.Invoke();
            }
        });
    }

    public void ResetPanel()
    {
        window.localScale = Vector3.zero;
        objParent.SetActive(false);
        inAnimation = false;
    }
}
