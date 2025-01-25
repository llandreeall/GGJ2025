using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public int increment;
    public BoxCollider2D boxColl;
    [SerializeField] private LayerMask mask;

    public bool CheckOverlap(Vector2 position)
    {
        Vector2 size = boxColl.size;
        // Check for overlaps at the given position
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(position, size, 0, mask);

        // Return true if there are any colliders overlapping
        return hitColliders.Length > 0;
    }
}
