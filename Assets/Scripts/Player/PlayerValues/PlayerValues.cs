using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerValues", menuName = "PlayerValues")]
public class PlayerValues : ScriptableObject
{
    [Header("General")]
    public float defaultGravity = 5f;
    public float defaultLinearDrag = 0f;
    [Space]

    [Header("Move State")]
    public float moveMaxVelocity = 10f;
    public float moveAccelerationSeconds = 0.1f;
    [Space]

    [Header("Aerial State")]
    public float airMoveMaxVelocity = 10f;
    public float airMoveAccelerationSeconds = 0.3f;
    [Space]


    [Header("Jump State")]
    public bool jumpCanPlayerFlip = true;
    public float jumpVelocity = 15f;
    [Space]

    [Header("Fall State")]
    public bool fallCanPlayerFlip = true;
    public float coyoteTime = 0.05f;
    public float fallForce = -5f;
    public float fallForceWhenGoingUp = -15f;
    public float fallTerminalVelocity = -10f;
    [Space]

    [Header("Dash State")]
    public float dashCooldownSeconds = 0.5f;
    public float dashVelocity = 30f;
    public float dashTime = 0.3f;
    public float dashLinearDrag = 8f;
    [Space]

    [Header("Bubble")]
    public Vector2 bubbleInitialSpawnOffset;
    public Vector2 bubbleInitialVelocity;
    public float bubbleGravityScale = -1f;
    public float bubbleLinearDrag = 8;
    public float bubbleMaxLifetimeSeconds = 10f;
    [Space]

    [Header("Death State")]
    public float deathToRespawnSeconds = 1f;
    [Space]

    [Header("Respawn State")]
    public float respawnToIdleSeconds = 0.5f;
    [Space]

    [Header("Checks")]
    public Vector2 groundCheckBox;
    public Vector2 groundCheckOffset = new Vector2(0, -1);

    public Vector2 rightWallCheckBox;
    public Vector2 rightWallCheckOffset = new Vector2(1, 0);

    public Vector2 leftWallCheckBox;
    public Vector2 leftWallCheckOffset = new Vector2(-1, 0);

    public Vector2 hazardCheckBox;
    public Vector2 hazardCheckOffset = new Vector2(0, 0);

    public LayerMask whatIsHazard;
    public LayerMask whatIsGround;

    [Header("Ability Unlocks")]
    public bool isDashUnlocked = false;
    public bool isBubbleUnlocked = false;
    public bool isExpandUnlocked = false;
}
