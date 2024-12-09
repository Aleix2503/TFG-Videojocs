using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "newPlayerPhysicsValues", menuName = "PlayerPhysicsValues")]
public class PlayerPhysicsValues : ScriptableObject
{
    [Header("General")]
    public float defaultGravity = 5f;
    public float defaultLinearDrag = 0f;

    public Vector2 defaultCollisionBox = Vector2.one;
    public Vector2 defaultCollisionBoxOffset = Vector2.zero;
    public float defaultCollisionEdgeRadius = 0.1f;
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
    public float jumpVelocity = 15f;
    [Space]

    [Header("Fall State")]
    public float fallForce = -5f;
    public float fallForceWhenGoingUp = -15f;
    public float fallTerminalVelocity = -10f;
    [Space]

    [Header("Dash State")]
    public float dashVelocity = 30f;
    public float dashTime = 0.3f;
    public float dashLinearDrag = 8f;
    [Space]

    [Header("Bubble State")]
    public float bubbleHorizontalVelocity = 5f;
    public float bubbleHorizontalAccelerationSeconds = 0.3f;
    public float floatForce = 5f;
    public float floatForceWhenGoingDown = 15f;
    public float floatTerminalVelocity = 10f;
    public float bubbleTransformationTime = 1f;
    public float bubbleBounceTime = 0.3f;
    public float bubbleVerticalBounceForce;
    public float bubbleHorizontalBounceForce;
    //[Header("Expansion State")]

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

}
