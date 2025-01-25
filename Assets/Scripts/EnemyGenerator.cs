using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    public EnemyPool enemyPool;

    [SerializeField] private GameplayManager gameplayManager;
    [SerializeField] private int poolSize = 3;
    [SerializeField] private List<EnemyInteractable> enemyPrefabList;
    [SerializeField] private Transform enemyParent;
    [SerializeField] private List<Transform> possiblePositions;
    [SerializeField] private float generationTimerTarget = 5;

    private float generationTimer;
    private bool doGenerate;

    private void Awake()
    {
        enemyPool = new EnemyPool(enemyPrefabList, enemyParent, poolSize);
        doGenerate = false;
    }

    public void StartGenerate()
    {
        doGenerate = true;
    }

    private void Update()
    {
        if (!gameplayManager.IsGameRunning() || !doGenerate) return;

        //Generate bubbles
        generationTimer += Time.deltaTime;
        if (generationTimer >= generationTimerTarget)
        {
            var obj = enemyPool.GetFirstAvailable();
            Vector2 pos = possiblePositions[Random.Range(0, possiblePositions.Count)].position;
            obj.InitializeEnemy(gameplayManager);

            //Dont overlap interactables
            obj.transform.position = pos;
            obj.SetDirection();
            generationTimer = 0;
        }
    }

    public void StopGenerator()
    {
        doGenerate = false;
        EnemyInteractable[] interactables = FindObjectsByType<EnemyInteractable>(FindObjectsSortMode.InstanceID);
        for (int i = 0; i < interactables.Length; i++)
        {
            enemyPool.ReturnToPool(interactables[i]);
        }
    }

    public void ResetGenerator()
    {
        generationTimer = 0;
    }

}
