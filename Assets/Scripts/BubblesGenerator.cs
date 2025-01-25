using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubblesGenerator : MonoBehaviour
{
    public BubblesPool bubblesPool;

    [SerializeField] private GameplayManager gameplayManager;
    [SerializeField] private int bubblesPoolSize = 5;
    [SerializeField] private BubblesInteractable bubblesPrefab;
    [SerializeField] private Transform bubblesParent;
    [SerializeField] private float minXGenerator;
    [SerializeField] private float maxXGenerator;
    [SerializeField] private float bubblesGenerationTimerTarget = 3;

    private float bubblesGenerationTimer;

    private void Awake()
    {
        bubblesPool = new BubblesPool(bubblesPrefab, bubblesParent, bubblesPoolSize);
    }

    private void Update()
    {
        if (!gameplayManager.isGameRunning) return;

        //Generate bubbles
        bubblesGenerationTimer += Time.deltaTime;
        if (bubblesGenerationTimer >= bubblesGenerationTimerTarget)
        {
            var obj = bubblesPool.GetRandomAvailable();
            Vector2 pos;
            obj.InitializeBubbles(gameplayManager);
            //Dont overlap interactables
            pos = CheckPositionsInLoop(obj);
            if(pos == Vector2.zero) pos = new Vector3(Random.Range(minXGenerator, maxXGenerator), bubblesParent.position.y, 0);
            obj.transform.position = pos;
            bubblesGenerationTimer = 0;
        }
    }

    public void ResetGenerator()
    {
        bubblesGenerationTimer = 0;
    }

    public Vector2 CheckPositionsInLoop(BubblesInteractable bubble)
    {
        Vector3 pos = new Vector3(Random.Range(minXGenerator, maxXGenerator), bubblesParent.position.y, 0);
        while (bubble.CheckOverlap(pos))
        {
            pos = new Vector3(Random.Range(minXGenerator, maxXGenerator), bubblesParent.position.y, 0);
        }
        return pos;
    }
}
