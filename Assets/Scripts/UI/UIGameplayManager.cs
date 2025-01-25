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
    private int lastBubbleIDToPop;

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
        lastBubbleIDToPop = uiBubblesList.Count - 1;
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

    public void PopBubbleUI(float id)
    {
        if (id > lastBubbleIDToPop)
        {
            for(int i = lastBubbleIDToPop + 1; i < id; i++)
            {
                uiBubblesList[i].Pop();
            }
        }
        else if (id == lastBubbleIDToPop)
        {
            if (lastBubbleIDToPop >= 0 && lastBubbleIDToPop <= uiBubblesList.Count - 1)
            {
                uiBubblesList[lastBubbleIDToPop].Pop();
                lastBubbleIDToPop--;
            }
        }
    }

    public void AddBubble(int value)
    {
        for (int i = 0; i < value; i++)
        {
            if (lastBubbleIDToPop <= uiBubblesList.Count - 1)
            {
                uiBubblesList[lastBubbleIDToPop].ResetFast();
                lastBubbleIDToPop++;
            }
        }
        if(lastBubbleIDToPop > uiBubblesList.Count - 1)
        {
            lastBubbleIDToPop = uiBubblesList.Count - 1;
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
        gameManager.RestartGame();
        endPanel.HidePanel();
        yield return new WaitForSeconds(SHRINK_TIME);
        gameManager.StartGame();
    }

    public IEnumerator ClickNext()
    {
        gameManager.RestartGame();
        endPanel.exitBttn.gameObject.SetActive(false);
        endPanel.exitBttn.gameObject.SetActive(false);
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
        lastBubbleIDToPop = Mathf.FloorToInt(oxygen + 1);
    }
}
