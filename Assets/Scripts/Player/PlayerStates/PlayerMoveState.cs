using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerState
{
    public PlayerMoveState(PlayerController playerController, PlayerStateMachine playerStateMachine, PlayerValues playerValues, string animBoolName) : base(playerController, playerStateMachine, playerValues, animBoolName)
    {
    }

    private float currentRelativeVelocity;

    private bool isTouchingFrontWall = false;
    private bool hasTouchedFrontWall = false;

    public override void Enter()
    {
        base.Enter();
        playerController.ResetGroundFlags();
        currentRelativeVelocity = playerController.m_rb2D.velocity.x/playerValues.moveMaxVelocity;

        playerController.SetPaintingState(PlayerPaintingState.moving);

        hasTouchedFrontWall = false;
    }



    public override void Update()
    {
        base.Update();

        playerController.CheckIfShouldFlip(playerController.m_playerInputHandler.absoluteMovementInput);

        playerController.SetVelocityX(CalculateNewVelocity());

        if (isTouchingFrontWall && !hasTouchedFrontWall)
        {
            PaintManager._instance.PlaceSplat(playerController.transform.position + new Vector3(0.5f * playerController.facingDirection, 0, 0), 
                Vector3.left * playerController.facingDirection, playerValues.defaultColor);

            hasTouchedFrontWall = true;
        }

        if (playerController.CheckIfCanDash())
        {
            playerStateMachine.ChangeState(playerController.dashState);
            return;
        }

        if (playerController.CheckIfCanBubble())
        {
            playerStateMachine.ChangeState(playerController.summonBubbleState);
            return;
        }

        if (playerController.CheckIfCanExpand())
        {
            playerStateMachine.ChangeState(playerController.startExpandState);
            return;
        }

        if (playerController.m_playerInputHandler.jumpInput == true)
        {
            playerController.m_playerInputHandler.UseJumpInput();
            playerStateMachine.ChangeState(playerController.jumpState);
        }

        if (playerController.m_playerInputHandler.absoluteMovementInput == 0)
        {
            playerStateMachine.ChangeState(playerController.idleState);
        }

        if (!isGrounded)
        {
            playerController.fallState.StartCoyoteTime();
            playerStateMachine.ChangeState(playerController.fallState);
        }
    }

    private float CalculateNewVelocity()
    {
        int movementInput = playerController.m_playerInputHandler.absoluteMovementInput;

        if (currentRelativeVelocity * movementInput < 0)
        {
            currentRelativeVelocity = 0;
        } else
        {
            currentRelativeVelocity += (Time.deltaTime / playerValues.moveAccelerationSeconds) * movementInput;
        }

        currentRelativeVelocity = Mathf.Clamp(currentRelativeVelocity, -1, 1);

        return playerValues.moveMaxVelocity * currentRelativeVelocity;
    }

    public override void Exit()
    {
        base.Exit();

        playerController.SetPaintingState(PlayerPaintingState.def);
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isTouchingFrontWall = playerController.checkIfTouchingFrontWall();
    }
}
