using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerValues", menuName = "PlayerValues")]
public class PlayerValues : ScriptableObject
{
    [Header("Move State")]
    public float moveMaxVelocity = 10f;
    public float moveAccelerationSeconds = 0.1f;
    
    [Header("Aerial State")]
    public float airMoveMaxVelocity = 10f;
    public float airMoveAccelerationSeconds = 0.3f;
    

    [Header("Jump State")]
    public float jumpVelocity = 15f;

    [Header("Fall State")]
    public float fallForce = -5f;
    public float fallForceWhenGoingUp = -15f;

    [Header("Checks")]
    public Vector2 groundCheckBox;
    public LayerMask whatIsGround;

    [Header("Ability Unlocks")]
    public bool isAbility1Unlocked = false;
    public bool isAbility2Unlocked = false;
    public bool isAbility3Unlocked = false;
}
