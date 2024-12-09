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

    [Header("Expand State")]
    public float expandColorFadeTime = 0.5f;
    public float expandBreakPlatformVelocityThreshold = -15;
    public bool expandedShowGizmos = false;
    [Space]

    [Header("Death State")]
    public float deathToRespawnSeconds = 1f;
    [Space]

    [Header("Respawn State")]
    public float respawnToIdleSeconds = 0.5f;
    [Space]

    [Header("Cheats")]
    public bool unlockAllAbilities = false;
}
