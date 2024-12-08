using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerControlValues", menuName = "PlayerControlValues")]
public class PlayerControlValues : ScriptableObject
{
    [Header("General")]
    public bool showGizmos = true;
    public float initialNoControlTime = 5f;
    public float abilityUnlockNoControlTime = 2f;

    public Color defaultColor = Color.white;
    public Color dashColor = Color.yellow;
    public Color bubbleColor = Color.cyan;
    public Color expandColor = Color.magenta;
    
    [Header("Jump State")]
    public float jumpBufferTime = 0.2f;
    [Space]

    [Header("Fall State")]
    public bool fallCanPlayerFlip = true;
    public float coyoteTime = 0.05f;
    [Space]

    [Header("Dash State")]
    public float dashCooldownSeconds = 0.5f;
    public float dashColorFadeInTime = 0.1f;
    public float dashColorFadeOutTime = 0.5f;
    [Space]

    [Header("Bubble")]
    public float bubbleTransformationTime = 1f;
    public int bubbleMaxBounces = 2;
    [Space]

    /*[Header("StartExpand State")]
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
    [Space]*/

    [Header("Death State")]
    public float deathToRespawnSeconds = 1f;
    [Space]

    [Header("Respawn State")]
    public float respawnToIdleSeconds = 0.5f;
    [Space]

    [Header("Cheats")]
    public bool unlockAllAbilities = false;
}
