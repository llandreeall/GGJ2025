using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInteractable : Interactable
{
    public float speed = 5;
    public float maxX = 30;
    public float minX = -30;
    public Transform art;
    private GameplayManager gameManager;
    private Vector3 direction = Vector3.left;
    private bool isRunning;
    Vector3 artScale;
    private void Awake()
    {
        artScale = art.localScale;
    }
    public void InitializeEnemy(GameplayManager manager)
    {
        gameManager = manager;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!gameManager.IsGameRunning() && !gameManager.IsPlayerPaused()) return;
        if(collision.tag == "Player")
        {
            //Hitplayer
            boxColl.enabled = false;
            gameManager.PlayerHit(increment);
        }
    }

    private void OnEnable()
    {
        boxColl.enabled = true;
        isRunning = true;
    }

    private void FixedUpdate()
    {
        if (!isRunning) return;
        Move();
        float x = transform.position.x;
        if (x >= maxX || x <= minX)
        {
            isRunning = false;
            gameManager.enemyGenerator.enemyPool.ReturnToPool(this);
        }
    }

    private void Move()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    public void SetDirection()
    {
        float x = transform.position.x;
        if (x > 0)
        {
            direction = Vector3.left;
            art.localScale = new Vector3(-artScale.x, artScale.y, artScale.z);
        }
        else
        {
            direction = Vector3.right;
            art.localScale = new Vector3(artScale.x, artScale.y, artScale.z);
        }
        isRunning = true;
    }

}
