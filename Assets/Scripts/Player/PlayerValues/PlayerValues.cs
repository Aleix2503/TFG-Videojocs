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
    public float coyoteTime = 0.05f;
    public float fallForce = -5f;
    public float fallForceWhenGoingUp = -15f;
    public float fallTerminalVelocity = -10f;

    [Header("Checks")]
    public Vector2 groundCheckBox;
    public LayerMask whatIsGround;

    [Header("Ability Unlocks")]
    public bool isDashUnlocked = false;
    public bool isBubbleUnlocked = false;
    public bool isExpandUnlocked = false;
}
