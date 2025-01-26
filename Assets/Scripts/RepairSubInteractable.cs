using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairSubInteractable : Interactable
{
    [SerializeField] private float repairTime;

    private GameplayManager gameManager;

    public void InitializeRepairSub(GameplayManager manager)
    {
        gameManager = manager;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //Repair ship
            StartCoroutine(RepairSub());
        }
    }

    private void OnEnable()
    {
        boxColl.enabled = true;
    }

    private void DisableRepairSub()
    {
        gameManager.repairSubGenerator.repairSubPool.ReturnToPool(this);
    }

    private IEnumerator RepairSub()
    {
        boxColl.enabled = false;
        gameManager.PausePlayer(true);
        yield return new WaitForSeconds(repairTime);
        gameManager.AddProgress(increment);
        DisableRepairSub();
        gameManager.PausePlayer(false);
    }
}
