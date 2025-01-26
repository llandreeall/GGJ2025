using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubblesInteractable : Interactable
{
    [SerializeField] private float lifeTimer;

    private float timer = -9999;
    private GameplayManager gameManager;
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
            GlobalGameManager.Instance.soundManager.PlaySound(SFXType.PlayerTakeBubbles);
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

    IEnumerator DisableBubbles()
    {
        timer = -9999;
        boxColl.enabled = false;
        bubbles.Stop();
        yield return new WaitForSeconds(1f);
        gameManager.bubblesGenerator.bubblesPool.ReturnToPool(this);
    }

    public void ResetBubble()
    {
        timer = -9999;
        boxColl.enabled = false;
        bubbles.Stop();
        gameManager.bubblesGenerator.bubblesPool.ReturnToPool(this);
    }
}
