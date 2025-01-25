using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public enum TransitionDirection
{
    IN,
    OUT
}

public class GlobalGameManager : Singleton<GlobalGameManager>
{
    public static string INTRO_SCENE_NAME = "IntroScene";
    public static string GAMEPLAY_SCENE_NAME = "GameplayScene";

    public float bestTime;
    public bool gameWon;
    [SerializeField] private Image transitionPanel;
    [SerializeField] private float transitionTime = 0.5f;

    protected override bool Initialize()
    {
        bestTime = 0;
        gameWon = false;
        return true;
    }

    public IEnumerator DoTransition(TransitionDirection dir, Action onCompleteAction = null)
    {
        Color transColor = transitionPanel.color;
        transColor.a = dir == TransitionDirection.IN ? 0 : 1;
        transitionPanel.gameObject.SetActive(true);
        transitionPanel.DOFade(dir == TransitionDirection.IN ? 1 : 0, transitionTime).OnComplete(delegate
        {
            if (onCompleteAction != null) onCompleteAction.Invoke();
            if(dir == TransitionDirection.OUT) transitionPanel.gameObject.SetActive(false);
        });
        yield return new WaitForSeconds(transitionTime);

    }

}
