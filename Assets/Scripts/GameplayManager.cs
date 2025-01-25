using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    private bool isGameRunning;
    public BubblesGenerator bubblesGenerator;
    private Action onGameStart;
    private Action onGameStop;

    [SerializeField] private float targetProgress;
    [SerializeField] private float targetOxygen;
    [SerializeField] private UIGameplayManager uiManager;
    
    private float currentProgress;
    private float currentOxygen;

    private void Awake()
    {
        ResetGame();
    }

    private void Start()
    {
        uiManager.InitializeUI();
    }

    public void ResetGame()
    {
        isGameRunning = false;
        currentProgress = 0;
        currentOxygen = targetOxygen;
    }

    private void Update()
    {
        //Debug
        if (Input.GetKeyDown(KeyCode.P))
            StartGame();
        if (Input.GetKeyDown(KeyCode.O))
            RestartGame();

        if (!isGameRunning) return;

        currentOxygen -= Time.deltaTime;
        //Update UI
        uiManager.PopBubbleUI(Mathf.FloorToInt(currentOxygen+1));
        if(currentOxygen <= 0)
        {
            currentOxygen = 0;
            //Stop Game
            StopGame();
        }
        
    }

    public void StartGame()
    {
        isGameRunning = true;
    }
    public void StopGame()
    {
        isGameRunning = false;
        Debug.Log("LOSE GAME");
    }

    public void RestartGame()
    {
        ResetGame();
        uiManager.InitializeUI();
    }

    public bool IsGameRunning()
    {
        return isGameRunning;
    }

    public void AddOnStartAction(Action onStart)
    {
        onGameStart += onStart;
    }

    public void AddBubble(int value)
    {
        if (uiManager.isInBubbleCoroutine) return;
        if (currentOxygen <= 0) return;
        currentOxygen += value;
        if (currentOxygen > targetOxygen) currentOxygen = targetOxygen;
        uiManager.AddBubble(value);
    }

}
