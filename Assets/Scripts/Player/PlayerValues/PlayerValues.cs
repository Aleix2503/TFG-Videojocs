using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerValues", menuName = "PlayerValues")]
public class PlayerValues : ScriptableObject
{
    [Header("General")]
    public bool showGizmos = true;
    public float defaultGravity = 5f;
    public float defaultLinearDrag = 0f;
    public float initialNoControlTime = 5f;
    public float abilityUnlockNoControlTime = 2f;

    public Vector2 defaultCollisionBox = Vector2.one;
    public Vector2 defaultCollisionBoxOffset = Vector2.zero;
    public float defaultCollisionEdgeRadius = 0.1f;

    public Color defaultColor = Color.white;
    public Color dashColor = Color.yellow;
    public Color bubbleColor = Color.cyan;
    public Color expandColor = Color.magenta;
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
    public float jumpBufferTime = 0.2f;
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
    public float dashColorFadeInTime = 0.1f;
    public float dashColorFadeOutTime = 0.5f;
    [Space]

    [Header("Bubble Summon State")]
    public float bubbleSummonDelay = 0.5f;
    public float bubbleSummonFinishTime = 1;
    public float bubbleSummonColorFadeInTime = 0.3f;
    public float bubbleSummonColorFadeOutTime = 0.5f;

    [Header("Bubble")]
    public Vector2 bubbleInitialSpawnOffset;
    public Vector2 bubbleInitialVelocity;
    public float bubbleGravityScale = -1f;
    public float bubbleLinearDrag = 8;
    public float bubbleMaxLifetimeSeconds = 10f;
    public float bubbledColorFadeInTime = 0.05f;
    public float bubbledColorFadeOutTime = 0.05f;
    [Space]

    [Header("StartExpand State")]
    public float startExpandTime = 0.5f;
    public float startExpandColorFadeInTime = 0.5f;
    [Space]

    [Header("Expand State")]
    public float expandMaxFallTime = 5;
    public float expandGravityMultiplier = 2;
    public float expandBreakPlatformVelocityThreshold = -15;
    [Space]
    public bool expandedShowGizmos = false;
    public Vector2 expandedCollisionBox = Vector2.one;
    public Vector2 expandedCollisionBoxOffset = Vector2.zero;
    public float expandedCollisionEdgeRadius = 0.1f;

    public Vector2 expandedGroundCheckBox = Vector2.one;
    public Vector2 expandedGroundCheckBoxOffset = Vector2.zero;

    public Vector2 expandedHazardCollisionBox = Vector2.one;
    public Vector2 expandedHazardCollisionBoxOffset = Vector2.zero;
    [Space]

    [Header("EndExpand State")]
    public float endExpandTime = 0.5f;
    public float endExpandColorFadeOutTime = 0.5f;
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

    public Vector2 ceilingCheckBox;
    public Vector2 ceilingCheckOffset = new Vector2(0, -1);

    public Vector2 hazardCheckBox;
    public Vector2 hazardCheckOffset = new Vector2(0, 0);

    public Vector2 bubbleCheckBox = new Vector2(1, 1);
    public Vector2 bubbleCheckOffset = new Vector2(0, 0);

    public LayerMask whatIsHazard;
    public LayerMask whatIsGround;
    public LayerMask whatIsBubble;

    [Header("Cheats")]
    public bool unlockAllAbilities = false;
}
