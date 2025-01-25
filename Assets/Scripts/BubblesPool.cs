using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubblesPool : ObjectPool<BubblesInteractable>
{
    public BubblesPool(BubblesInteractable bubblePrefab, Transform parentTransform, int startSize = 1) : base(bubblePrefab, parentTransform, startSize)
    {

    }
}
