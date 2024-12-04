using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysics : MonoBehaviour
{
    public PlayerPhysicsValues playerPhysicsValues;

    public Rigidbody2D rb2D;
    public BoxCollider2D bCollider;

    public int facingDirection { get; private set; }
    public bool isGrounded => checkIfGrounded();


    public void Start()
    {
        SetGravityScale(playerPhysicsValues.defaultGravity);
        SetColliderDimensions(playerPhysicsValues.defaultCollisionBox, playerPhysicsValues.defaultCollisionBoxOffset, playerPhysicsValues.defaultCollisionEdgeRadius);
    }
    #region Physics Settings
    public void SetVelocityX(float velocity)
    {
        Vector2 newVelocity = new(velocity, rb2D.velocity.y);
        rb2D.velocity = newVelocity;
    }

    public void SetVelocityY(float velocity)
    {
        Vector2 newVelocity = new(rb2D.velocity.x, velocity);
        rb2D.velocity = newVelocity;
    }

    public void SetGravityScale(float gravity)
    {
        rb2D.gravityScale = gravity;
    }

    public void SetLinearDrag(float linearDrag)
    {
        rb2D.drag = linearDrag;
    }
    public void SetColliderDimensions(Vector2 size, Vector2 offset, float edgeRadius)
    {
        bCollider.size = size;
        bCollider.offset = offset;
        bCollider.edgeRadius = edgeRadius;
    }
    #endregion
    #region Physics Movement
    public void FreezePlayerPosition(bool isPlayerFrozen)
    {
        if (isPlayerFrozen)
        {
            rb2D.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else
        {
            rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }
    public void Stop()
    {

    }
    public void Move()
    {

    }

    public void Jump()
    {

    }
    #endregion
    #region Physics Checks
    public bool checkIfGrounded()
    {
        return Physics2D.OverlapBox((Vector2)transform.position + playerPhysicsValues.groundCheckOffset, playerPhysicsValues.groundCheckBox, 0, playerPhysicsValues.whatIsGround);
    }
    public bool checkIfTouchingFrontWall()
    {
        if (facingDirection == 1)
        {
            return checkIfTouchingRightWall();
        }
        else
        {
            return checkIfTouchingLeftWall();
        }
    }

    public bool checkIfTouchingRightWall()
    {
        return Physics2D.OverlapBox((Vector2)transform.position + playerPhysicsValues.rightWallCheckOffset, playerPhysicsValues.rightWallCheckBox, 0, playerPhysicsValues.whatIsGround);
    }

    public bool checkIfTouchingLeftWall()
    {
        return Physics2D.OverlapBox((Vector2)transform.position + playerPhysicsValues.leftWallCheckOffset, playerPhysicsValues.leftWallCheckBox, 0, playerPhysicsValues.whatIsGround);
    }

    public bool checkIfTouchingCeiling()
    {
        return Physics2D.OverlapBox((Vector2)transform.position + playerPhysicsValues.ceilingCheckOffset, playerPhysicsValues.ceilingCheckBox, 0, playerPhysicsValues.whatIsGround);
    }

    public bool checkIfTouchingHazard()
    {
        return Physics2D.OverlapBox((Vector2)transform.position + playerPhysicsValues.hazardCheckOffset, playerPhysicsValues.hazardCheckBox, 0, playerPhysicsValues.whatIsHazard);
    }

    /*public bool checkIfTouchingBubble()
    {
        return Physics2D.OverlapBox((Vector2)transform.position + playerPhysicsValues.bubbleCheckOffset, playerPhysicsValues.bubbleCheckBox, 0, playerPhysicsValues.whatIsBubble);
    }

    public bool checkIfExpandedCollision()
    {
        return Physics2D.OverlapBox((Vector2)transform.position + playerPhysicsValues.expandedCollisionBoxOffset, playerPhysicsValues.expandedCollisionBox, 0, playerPhysicsValues.whatIsGround);
    }

    public bool checkIfExpandedTouchingHazard()
    {
        return Physics2D.OverlapBox((Vector2)transform.position + playerPhysicsValues.expandedHazardCollisionBoxOffset, playerPhysicsValues.expandedHazardCollisionBox, 0, playerPhysicsValues.whatIsHazard);
    }

    public bool checkIfExpandedTouchingGround()
    {
        return Physics2D.OverlapBox((Vector2)transform.position + playerPhysicsValues.expandedGroundCheckBoxOffset, playerPhysicsValues.expandedGroundCheckBox, 0, playerPhysicsValues.whatIsGround);
    }*/
    #endregion
}
