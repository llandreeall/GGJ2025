using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    private bool isGameRunning;
    public BubblesGenerator bubblesGenerator;
    public RepairSubGenerator repairSubGenerator;
    private Action onGameStart;
    private Action onGameStop;

    [SerializeField] private float targetProgress;
    [SerializeField] private float targetOxygen;
    [SerializeField] private UIGameplayManager uiManager;
    [SerializeField] private PlayerController player;
    [SerializeField] private float timeSpeed = 0.5f;
    
    private float currentProgress;
    private float currentOxygen;

    private void Awake()
    {
        ResetGame();
    }

    private void Start()
    {
        uiManager.InitializeUI(targetProgress);
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

        currentOxygen -= Time.deltaTime * timeSpeed;
        //Update UI
        uiManager.PopBubbleUI(Mathf.FloorToInt(currentOxygen+1));
        if(currentOxygen <= 0)
        {
            currentOxygen = 0;
            //Stop Game
            Debug.Log("LOSE GAME");
            StopGame();
        }
        
    }

    public void StartGame()
    {
        isGameRunning = true;
        repairSubGenerator.StartGenerate();
    }

    public void StopGame()
    {
        isGameRunning = false;
        repairSubGenerator.StopGenerator();
        bubblesGenerator.StopGenerator();
        player.ResetPlayer();
    }

    public void RestartGame()
    {
        ResetGame();
        uiManager.InitializeUI(targetProgress);
        bubblesGenerator.ResetGenerator();
        repairSubGenerator.ResetGenerator();
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

    public void AddProgress(int val)
    {
        currentProgress = currentProgress + val >= targetProgress ? targetProgress : currentProgress + val;
        uiManager.UpdateProgressSlider(currentProgress);
        repairSubGenerator.StartGenerate();
        if(currentProgress >= targetProgress)
        {
            //WIN
            Debug.Log("WIN GAME");
            StopGame();
        }
    }

    public void PausePlayer(bool isPaused)
    {
        player.PausePlayer(isPaused);
    }
}
