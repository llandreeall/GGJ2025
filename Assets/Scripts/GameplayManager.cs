using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    private bool isGameRunning;
    public BubblesGenerator bubblesGenerator;
    public RepairSubGenerator repairSubGenerator;
    public EnemyGenerator enemyGenerator;

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
        StartCoroutine(GlobalGameManager.Instance.DoTransition(TransitionDirection.OUT, () =>
        {
            uiManager.InitializeUI(targetProgress);
            uiManager.ShowInstructions();
        }));
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
        /*
        if (Input.GetKeyDown(KeyCode.P))
            StartGame();
        if (Input.GetKeyDown(KeyCode.O))
            RestartGame();*/

        if (!isGameRunning) return;

        currentOxygen -= Time.deltaTime * timeSpeed;
        //Update UI
        uiManager.PopBubbleUI(currentOxygen+1);
        if(currentOxygen <= 0)
        {
            currentOxygen = 0;
            //Stop Game
            Debug.Log("LOSE GAME");
            StartCoroutine(LoseCoroutine());
        }
        
    }

    public void StartGame()
    {
        isGameRunning = true;
        repairSubGenerator.StartGenerate();
        enemyGenerator.StartGenerate();
    }

    public void StopGame()
    {
        isGameRunning = false;
        repairSubGenerator.StopGenerator();
        bubblesGenerator.StopGenerator();
        enemyGenerator.StopGenerator();
        player.ResetPlayer();
    }

    public void RestartGame()
    {
        ResetGame();
        uiManager.InitializeUI(targetProgress);
        bubblesGenerator.ResetGenerator();
        repairSubGenerator.ResetGenerator();
        enemyGenerator.ResetGenerator();
    }

    public bool IsGameRunning()
    {
        return isGameRunning;
    }

    public bool IsPlayerPaused()
    {
        return player.playerPaused;
    }

    public void AddBubble(int value)
    {
        if (uiManager.isInBubbleCoroutine) return;
        if (currentOxygen <= 0) return;
        currentOxygen += value;
        if (currentOxygen > targetOxygen) currentOxygen = targetOxygen;
        uiManager.AddBubble(currentOxygen);
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
            StartCoroutine(WinCoroutine());
        }
    }

    IEnumerator WinCoroutine()
    {
        StopGame();
        player.ResetPlayer();
        yield return new WaitForSeconds(0.7f);
        uiManager.PrepapeEndPanel(true, 0);
        uiManager.ShowEndPanel();
    }

    IEnumerator LoseCoroutine()
    {
        StopGame();
        player.ResetPlayer();
        yield return new WaitForSeconds(0.7f);
        uiManager.PrepapeEndPanel(false, 0);
        uiManager.ShowEndPanel();
    }


    public void PausePlayer(bool isPaused)
    {
        player.PausePlayer(isPaused);
    }

    public void PlayerHit(int val)
    {
        currentOxygen -= val;
        player.HitParticles();
        uiManager.SetBubblesAfterHit(currentOxygen);
    }
}
