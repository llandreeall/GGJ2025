using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : ObjectPool<EnemyInteractable>
{
    public EnemyPool(List<EnemyInteractable> enemyPrefab, Transform parentTransform, int startSize = 1) : base(enemyPrefab, parentTransform, startSize)
    {

    }
}
