using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndPanel : UIPanel
{
    public Button replayBttn;
    public Button exitBttn;

    [SerializeField]
    private GameObject winArt;
    [SerializeField]
    private GameObject loseArt;

    public void PreparePopupBeforeDisplay(bool hasWon, float timeRecord)
    {
        winArt.SetActive(hasWon);
        loseArt.SetActive(!hasWon);
        exitBttn.gameObject.SetActive(hasWon);
    }

    public void AddReplayAction(UnityEngine.Events.UnityAction replayAction)
    {
        if(replayBttn != null)
        {
            replayBttn.onClick.RemoveAllListeners();
            replayBttn.onClick.AddListener(replayAction);
        }
    }

    public void AddNextAction(UnityEngine.Events.UnityAction nextAction)
    {
        if (exitBttn != null)
        {
            exitBttn.onClick.RemoveAllListeners();
            exitBttn.onClick.AddListener(nextAction);
        }
    }

    public void PreparePopupButtons(UnityEngine.Events.UnityAction replayAction, UnityEngine.Events.UnityAction nextAction)
    {
        AddReplayAction(replayAction);
        AddNextAction(nextAction);
    }
}
