using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleController : MonoBehaviour
{
    public LayerMask disableWhenTouchingLayer;

    public Rigidbody2D rb2D;
    public Animator animator;
    public new Collider2D collider;
    public SpriteRenderer spriteRenderer;

    private PlayerController playerController;
    private Transform playerTransform;
    private PlayerValues playerValues;

    private float startTime;
    private float maxLifetime;

    public bool isActive { get; private set; }

    public void Initialize(PlayerController playerController, Transform playerTransform, PlayerValues playerValues)
    {
        this.playerController = playerController;
        this.playerTransform = playerTransform;
        this.playerValues = playerValues;

        this.spriteRenderer.color = Color.clear;

        isActive = false;
    }

    public void Update()
    {
        if (playerValues == null) return;
        if (isActive == false) return;

        if (Time.time > startTime + maxLifetime)
        {
            PopBubble();
        }
    }

    public void SummonBubble()
    {
        if (playerValues == null) return;
        rb2D.bodyType = RigidbodyType2D.Dynamic;

        this.spriteRenderer.color = playerValues.bubbleColor;

        Vector2 spawnPosition = new Vector2(playerTransform.position.x + playerValues.bubbleInitialSpawnOffset.x * playerController.facingDirection,
            playerTransform.position.y + playerValues.bubbleInitialSpawnOffset.y);

        transform.position = spawnPosition;
        rb2D.velocity = playerValues.bubbleInitialVelocity;
        rb2D.gravityScale = playerValues.bubbleGravityScale;
        rb2D.drag = playerValues.bubbleLinearDrag;

        startTime = Time.time;
        maxLifetime = playerValues.bubbleMaxLifetimeSeconds;

        collider.enabled = true;

        isActive = true;

        animator.SetBool("pop", false);
        animator.SetBool("spawn", true);
    }

    private void OnDisable()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int collisionLayerMask = 1 << collision.gameObject.layer;

        if ((disableWhenTouchingLayer.value & collisionLayerMask) != 0)
        {
            PopBubble();
        }
    }

    internal void PopBubble()
    {
        isActive = false;
        collider.enabled = false;
        rb2D.bodyType = RigidbodyType2D.Static;

        animator.SetBool("pop", true);
        animator.SetBool("spawn", false);
    }
}
