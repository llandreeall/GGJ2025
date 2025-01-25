using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubblesInteractable : Interactable
{
    [SerializeField] private float lifeTimer;

    private float timer = -9999;
    private GameplayManager gameManager;
    [SerializeField] private LayerMask mask;
    [SerializeField] private ParticleSystem bubbles;

    public void InitializeBubbles(GameplayManager manager)
    {
        gameManager = manager;
        timer = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            gameManager.AddBubble(increment);
            StartCoroutine(DisableBubbles());
        }
    }

    private void Update()
    {
        if(timer >= 0 && gameManager.IsGameRunning())
        {
            timer += Time.deltaTime;
            if(timer >= lifeTimer)
            {
                //Make bubbles dissapear
                StartCoroutine(DisableBubbles());
            }
        }
    }

    private void OnEnable()
    {
        timer = -9999;
        boxColl.enabled = true;
        bubbles.Play();
    }

    public bool CheckOverlap(Vector2 position)
    {
        Vector2 size = boxColl.size;
        // Check for overlaps at the given position
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(position, size, 0, mask);

        // Return true if there are any colliders overlapping
        return hitColliders.Length > 0;
    }

    IEnumerator DisableBubbles()
    {
        timer = -9999;
        boxColl.enabled = false;
        bubbles.Stop();
        yield return new WaitForSeconds(1f);
        gameManager.bubblesGenerator.bubblesPool.ReturnToPool(this);
    }
}
