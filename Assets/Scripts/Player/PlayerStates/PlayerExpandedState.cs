using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExpandedState : PlayerState
{
    public PlayerExpandedState(PlayerController playerController, PlayerStateMachine playerStateMachine, PlayerValues playerValues, string animBoolName) : base(playerController, playerStateMachine, playerValues, animBoolName)
    {
    }

    bool expandedIsTouchingGround = false;
    bool expandedIsTouchingHazard = false;
    bool expandedIsGrounded = false;

    public bool canBreakGround = false;
    public override void DoChecks()
    {
        base.DoChecks();
        expandedIsTouchingGround = playerController.checkIfExpandedCollision();
        expandedIsTouchingHazard = playerController.checkIfExpandedTouchingHazard();
        expandedIsGrounded = playerController.checkIfExpandedTouchingGround();
    }

    public override void Enter()
    {
        base.Enter();
        playerController.SetColliderDimensions(playerValues.expandedCollisionBox, playerValues.expandedCollisionBoxOffset, playerValues.expandedCollisionEdgeRadius);
        playerController.SetGravityScale(playerValues.defaultGravity * playerValues.expandGravityMultiplier);

        canBreakGround = false;
    }

    public override void Exit()
    {
        base.Exit();
        playerController.SetColliderDimensions(playerValues.defaultCollisionBox, playerValues.defaultCollisionBoxOffset, playerValues.defaultCollisionEdgeRadius);
        playerController.SetGravityScale(playerValues.defaultGravity);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (playerController.m_rb2D.velocity.y  < playerValues.expandBreakPlatformVelocityThreshold)
        {
            canBreakGround = true;
        }
    }

    public override void Update()
    {
        base.Update();

        if (Time.time > startTime + playerValues.expandMaxFallTime)
        {
            playerStateMachine.ChangeState(playerController.endExpandState);
        }
        else if (expandedIsTouchingGround || expandedIsGrounded)
        {
            playerStateMachine.ChangeState(playerController.endExpandState);
        } else if (expandedIsTouchingHazard)
        {
            playerStateMachine.ChangeState(playerController.deathState); //TODO special death state?
        }

    }
}
