using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerValues", menuName = "PlayerValues")]
public class PlayerValues : ScriptableObject
{
    [Header("Move State")]
    public float moveSpeed = 10f;

    [Header("Jump State")]
    public float jumpVelocity = 15f;

    [Header("Fall State")]
    public float fallGravityMultiplierWhileGoingUp = 5f;

    [Header("Checks")]
    public Vector2 groundCheckBox;
    public LayerMask whatIsGround;

    [Header("Ability Unlocks")]
    public bool isAbility1Unlocked = false;
    public bool isAbility2Unlocked = false;
    public bool isAbility3Unlocked = false;
}
