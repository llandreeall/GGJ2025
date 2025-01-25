using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairSubPool : ObjectPool<RepairSubInteractable>
{
    public RepairSubPool(List<RepairSubInteractable> repairSubPrefab, Transform parentTransform, int startSize = 1) : base(repairSubPrefab, parentTransform, startSize)
    {

    }
}
