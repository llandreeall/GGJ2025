using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class UIGameplayManager : MonoBehaviour
{
    public static float OVERFLOW_TIME = 0.15f;
    public static float SHRINK_TIME = 0.09f;
    public static float GROWBACK_TIME = 0.04f;
    public static float COLOR_TIME = 0.3f;
    public static float TRANSITION_TIME = 0.3f;
    public bool isInBubbleCoroutine;
    [SerializeField]
    private Slider progressSlider;
    [SerializeField]
    private List<UIBubble> uiBubblesList;
    [SerializeField]
    private GameplayManager gameManager;
    [SerializeField]
    private UIPanel instructionsPanel;
    [SerializeField]
    private EndPanel endPanel;

    private Coroutine initializationCoroutine = null;

    private void Awake()
    {
        instructionsPanel.onPanelClosed += gameManager.StartGame;
    }

    private void Start()
    {
        endPanel.PreparePopupButtons(() => {
            StartCoroutine(ClickReplay());
        }, () => { StartCoroutine(ClickNext()); });
    }

    public void InitializeUI(float sliderMaxVal)
    {
        isInBubbleCoroutine = false;
        if(initializationCoroutine == null)
            initializationCoroutine = StartCoroutine(InitializeUICoroutine());
        
        ResetSlider(sliderMaxVal);
    }

    private IEnumerator InitializeUICoroutine()
    {
        isInBubbleCoroutine = true;
        foreach (UIBubble bubble in uiBubblesList)
        {
            bubble.Initialize();
            yield return new WaitForSeconds(0.1f);
        }
        initializationCoroutine = null;
        isInBubbleCoroutine = false;
    }

    public void SetBubbleStatus(float oxygen)
    {
        for (int i = 0; i < uiBubblesList.Count; i++)
        {
            if (i > oxygen - 1)
            {
                uiBubblesList[i].Pop();
            }
            else
            {
                uiBubblesList[i].Inflate();
            }
        }
    }

    public void UpdateProgressSlider(float val)
    {
        progressSlider.DOValue(val, TRANSITION_TIME);
    }

    public void ResetSlider(float maxValue)
    {
        progressSlider.value = 0;
        progressSlider.maxValue = maxValue;
    }

    public void ShowInstructions()
    {
        instructionsPanel.ShowPanel();
    }

    public IEnumerator ClickReplay()
    {
        for (int i = 0; i < uiBubblesList.Count; i++)
        {
            uiBubblesList[i].Pop();
        }
        GlobalGameManager.Instance.soundManager.PlaySound(SFXType.UI_Bttn);
        gameManager.RestartGame();
        endPanel.HidePanel();
        yield return StartCoroutine(InitializeUICoroutine());
        gameManager.StartGame();
    }

    public IEnumerator ClickNext()
    {
        GlobalGameManager.Instance.soundManager.PlaySound(SFXType.UI_Bttn);
        gameManager.RestartGame();
        endPanel.exitBttn.gameObject.SetActive(false);
        endPanel.replayBttn.gameObject.SetActive(false);
        yield return new WaitForSeconds(SHRINK_TIME);
        //Go to next scene
        StartCoroutine(GlobalGameManager.Instance.DoTransition(TransitionDirection.IN, () =>
        {
            GlobalGameManager.Instance.gameWon = true;
            SceneManager.LoadSceneAsync(GlobalGameManager.INTRO_SCENE_NAME);
        }));
    }

    public void ShowEndPanel()
    {
        endPanel.ShowPanel();
    }

    public void PrepapeEndPanel(bool hasWon, float timeRecord)
    {
        endPanel.PreparePopupBeforeDisplay(hasWon, timeRecord);
    }

    public void SetBubblesAfterHit(float oxygen)
    {
        for(int i = 0; i < uiBubblesList.Count; i++)
        {
            if(i + 1 > oxygen)
            {
                uiBubblesList[i].Pop();
            }
        }
    }
}
