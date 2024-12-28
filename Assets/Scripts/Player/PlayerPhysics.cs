using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysics : MonoBehaviour
{
    public PlayerPhysicsValues playerPhysicsValues;

    public Rigidbody2D rb2D;
    public BoxCollider2D bCollider;

    public PhysicsMaterial2D bounceMaterial;
    public PhysicsMaterial2D defaultMaterial;

    public int facingDirection { get; private set; }
    public bool isGrounded => checkIfGrounded();

    public bool isInkstink = false;

    private AttackPlayer attackPlayer;


    public void Start()
    {
        SetGravityScale(playerPhysicsValues.defaultGravity);
        SetColliderDimensions(playerPhysicsValues.defaultCollisionBox, playerPhysicsValues.defaultCollisionBoxOffset, playerPhysicsValues.defaultCollisionEdgeRadius);

        attackPlayer = GetComponent<AttackPlayer>();
        
        facingDirection = 1;
    }
    #region Physics Settings
    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }
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
        SetVelocityX(0);
        SetVelocityY(0);
    }
    public float AirMove(float movementDir,float currentRelativeVelocity)
    {
        CheckIfShouldFlip(movementDir);
        if (currentRelativeVelocity * movementDir < 0 || movementDir == 0)
        {
            currentRelativeVelocity = 0;
        }
        else
        {
            currentRelativeVelocity += (Time.deltaTime / playerPhysicsValues.airMoveAccelerationSeconds) * movementDir;
        }

        currentRelativeVelocity = Mathf.Clamp(currentRelativeVelocity, -1, 1);
        SetVelocityX(playerPhysicsValues.airMoveMaxVelocity * currentRelativeVelocity);
        return currentRelativeVelocity;
    }
    public float FloorMove(float movementDir, float currentRelativeVelocity)
    {
        CheckIfShouldFlip(movementDir);

        if (currentRelativeVelocity * movementDir < 0)
        {
            currentRelativeVelocity = 0;
        }
        else
        {
            currentRelativeVelocity += (Time.deltaTime / playerPhysicsValues.moveAccelerationSeconds) * movementDir;
        }

        currentRelativeVelocity = Mathf.Clamp(currentRelativeVelocity, -1, 1);
        if(isInkstink)
        {
            SetVelocityX(playerPhysicsValues.moveMaxVelocity * currentRelativeVelocity*playerPhysicsValues.inkstinkMultiplier);
        }
        else
        {
            SetVelocityX(playerPhysicsValues.moveMaxVelocity * currentRelativeVelocity);
        }
        
        return currentRelativeVelocity;
    }
    public void Jump()
    {
        SetVelocityY(playerPhysicsValues.jumpVelocity);
    }
    public void Fall()
    {
        rb2D.AddForce(new Vector2(0, playerPhysicsValues.fallForce));
    }
    public void BackToEarth()
    {
        rb2D.AddForce(new Vector2(0, playerPhysicsValues.fallForceWhenGoingUp));
        if (rb2D.velocity.y < 0)
        {
            SetVelocityY(0);
        }
    }
    public void FallMaxSpeed()
    {
        SetVelocityY(playerPhysicsValues.fallTerminalVelocity);
    }
    private void Flip()
    {
        facingDirection *= -1;
        transform.Rotate(0, 180, 0);
    }
    public void DashIn()
    {
        SetVelocityY(0);
        SetGravityScale(0);
        SetVelocityX(playerPhysicsValues.dashVelocity * facingDirection);
        SetLinearDrag(playerPhysicsValues.dashLinearDrag);
    }
    public void DashOut()
    {
        SetGravityScale(playerPhysicsValues.defaultGravity);
        SetLinearDrag(playerPhysicsValues.defaultLinearDrag);
    }
    public void BubbleIn()
    {
        FreezePlayerPosition(true);
        SetGravityScale(0);
        StartCoroutine(BubbleInCoroutine());
    }
    private IEnumerator BubbleInCoroutine()
    {
        yield return new WaitForSeconds(playerPhysicsValues.bubbleTransformationTime);
        FreezePlayerPosition(false);
    }
    public void BubbleOut()
    {
        FreezePlayerPosition(true);
    }
    public void ImpulseBubble()
    {
        rb2D.AddForce(new Vector2(0, playerPhysicsValues.floatForceWhenGoingDown),ForceMode2D.Impulse);
    }
    public void Float()
    {
        rb2D.AddForce(new Vector2(0, playerPhysicsValues.floatForce), ForceMode2D.Impulse);
    }
    public void BubbleMaxSpeed()
    {
        SetVelocityY(playerPhysicsValues.floatTerminalVelocity);
    }
    public float BubbleMove(int movementDir,float currentRelativeVelocity)
    {
        CheckIfShouldFlip(movementDir);
        if (currentRelativeVelocity * movementDir < 0 || movementDir == 0)
        {
            currentRelativeVelocity = 0;
        }
        else
        {
            currentRelativeVelocity += (Time.deltaTime / playerPhysicsValues.bubbleHorizontalAccelerationSeconds) * movementDir;
        }

        currentRelativeVelocity = Mathf.Clamp(currentRelativeVelocity, -1, 1);
        SetVelocityX(playerPhysicsValues.bubbleHorizontalVelocity * currentRelativeVelocity);
        return currentRelativeVelocity;
    }
    public bool BounceHorizontal(float destiny)
    {
        float position = rb2D.position.x;
        while (Math.Abs(position - destiny) > 0)
        {
            position = rb2D.position.x;
            rb2D.AddForce(new Vector2(-facingDirection * playerPhysicsValues.bubbleHorizontalBounceForce, 0),ForceMode2D.Impulse);
            return true;
        }
        return false;
    }
    public bool BounceVertical(float destiny)
    {
        float position = rb2D.position.y;
        if (position - destiny > 0)
        {
            rb2D.AddForce(new(0, -playerPhysicsValues.bubbleVerticalBounceForce),ForceMode2D.Impulse);
            return true;
        }
        return false;
    }
    public void CubeIn()
    {
        FreezePlayerPosition(true);
        StartCoroutine(CubeInCoroutine());
    }
    private IEnumerator CubeInCoroutine()
    {
        yield return new WaitForSeconds(playerPhysicsValues.cubedTime);
        FreezePlayerPosition(false);
        SetColliderDimensions(playerPhysicsValues.cubedCollisionBox, playerPhysicsValues.cubedCollisionBoxOffset, playerPhysicsValues.cubedCollisionEdgeRadius);
    }
    public void CubeOut()
    {
        FreezePlayerPosition(true);
        FreezePlayerPosition(false);
        SetColliderDimensions(playerPhysicsValues.defaultCollisionBox, playerPhysicsValues.defaultCollisionBoxOffset, playerPhysicsValues.defaultCollisionEdgeRadius);
    }
    public void CubedFall()
    {
        SetVelocityY(-playerPhysicsValues.cubedVerticalVelocity);
        SetVelocityY(-playerPhysicsValues.cubedVerticalVelocity);
    }
    #endregion
    #region Physics Checks

    public void CheckIfShouldFlip(float movementInput)
    {
        if (movementInput == 0) return;

        int direction = movementInput > 0 ? 1 : -1;

        if (direction != facingDirection && !attackPlayer.isAttacking)
        {
            Flip();
        }
    }
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

    public bool checkIfCubedCollision()
    {
        return Physics2D.OverlapBox((Vector2)transform.position + playerPhysicsValues.cubedCollisionBoxOffset, playerPhysicsValues.cubedCollisionBox, 0, playerPhysicsValues.whatIsGround);
    }

    public bool checkIfCubedTouchingHazard()
    {
        return Physics2D.OverlapBox((Vector2)transform.position + playerPhysicsValues.cubedHazardCollisionBoxOffset, playerPhysicsValues.cubedHazardCollisionBox, 0, playerPhysicsValues.whatIsHazard);
    }

    public bool checkIfCubedTouchingGround()
    {
        return Physics2D.OverlapBox((Vector2)transform.position + playerPhysicsValues.cubedGroundCheckBoxOffset, playerPhysicsValues.cubedGroundCheckBox, 0, playerPhysicsValues.whatIsGround);
    }
    #endregion
}
