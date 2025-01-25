using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private Coroutine initializationCoroutine = null;
    private int lastBubbleIDToPop;

    public void InitializeUI()
    {
        isInBubbleCoroutine = false;
        if(initializationCoroutine == null)
            initializationCoroutine = StartCoroutine(InitializeUICoroutine());
        lastBubbleIDToPop = uiBubblesList.Count - 1;
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
}
