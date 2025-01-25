using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public bool isGameRunning;
    public BubblesGenerator bubblesGenerator;

    [SerializeField] private float targetProgress;
    [SerializeField] private float targetOxygen;
    
    private float currentProgress;
    private float currentOxygen;

    private void Awake()
    {
        ResetGame();
    }

    public void ResetGame()
    {
        isGameRunning = false;
        currentProgress = 0;
        currentOxygen = 0;
    }

    private void Update()
    {
        if (!isGameRunning) return;

        
    }

}
