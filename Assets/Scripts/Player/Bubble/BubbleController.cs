using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleController : MonoBehaviour
{
    public LayerMask disableWhenTouchingLayer;

    public Rigidbody2D rb2D;

    private PlayerController playerController;
    private Transform playerTransform;
    private PlayerValues playerValues;

    private float startTime;
    private float maxLifetime;

    public void Initialize(PlayerController playerController, Transform playerTransform, PlayerValues playerValues)
    {
        this.playerController = playerController;
        this.playerTransform = playerTransform;
        this.playerValues = playerValues;
    }

    public void Update()
    {
        if (playerValues == null) return;

        if (Time.time > startTime + maxLifetime)
        {
            popBubble();
        }
    }

    private void OnEnable()
    {
        if (playerValues == null) return;

        Vector2 spawnPosition = new Vector2(playerTransform.position.x + playerValues.bubbleInitialSpawnOffset.x * playerController.facingDirection, 
            playerTransform.position.y + playerValues.bubbleInitialSpawnOffset.y);

        transform.position = spawnPosition;
        rb2D.velocity = playerValues.bubbleInitialVelocity;
        rb2D.gravityScale = playerValues.bubbleGravityScale;
        rb2D.drag = playerValues.bubbleLinearDrag;

        startTime = Time.time;
        maxLifetime = playerValues.bubbleMaxLifetimeSeconds;
    }

    private void OnDisable()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int collisionLayerMask = 1 << collision.gameObject.layer;

        if ((disableWhenTouchingLayer.value & collisionLayerMask) != 0)
        {
            popBubble();
        }
    }

    internal void popBubble()
    {
        gameObject.SetActive(false);
    }
}
