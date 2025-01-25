using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairSubGenerator : MonoBehaviour
{
    public RepairSubPool repairSubPool;

    [SerializeField] private GameplayManager gameplayManager;
    [SerializeField] private int poolSize = 5;
    [SerializeField] private List<RepairSubInteractable> repairSubPrefabList;
    [SerializeField] private Transform repairSubParent;
    [SerializeField] private List<Transform> possiblePositions;
    [SerializeField] private float generationTimerTarget = 3;

    private float generationTimer;
    private bool doGenerate;

    private void Awake()
    {
        repairSubPool = new RepairSubPool(repairSubPrefabList, repairSubParent, poolSize);
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
            var obj = repairSubPool.GetFirstAvailable();
            Vector2 pos;
            obj.InitializeRepairSub(gameplayManager);

            //Dont overlap interactables
            pos = CheckPositionsInLoop(obj);
            obj.transform.position = pos;
            generationTimer = 0;
            if (repairSubPool.GetUseObjects().Count == poolSize) doGenerate = false;
            else doGenerate = true;
        }
    }

    public void StopGenerator()
    {
        doGenerate = false;
        RepairSubInteractable[] interactables = FindObjectsByType<RepairSubInteractable>(FindObjectsSortMode.InstanceID);
        for(int i = 0; i < interactables.Length; i++)
        {
            repairSubPool.ReturnToPool(interactables[i]);
        }
    }

    public void ResetGenerator()
    {
        generationTimer = 1;
    }

    public Vector2 CheckPositionsInLoop(RepairSubInteractable repairSub)
    {
        Vector3 pos = possiblePositions[Random.Range(0, possiblePositions.Count)].position;
        while (repairSub.CheckOverlap(pos))
        {
            pos = possiblePositions[Random.Range(0, possiblePositions.Count)].position;
        }
        return pos;
    }
}
