using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerState
{
    public PlayerMoveState(PlayerController playerController, PlayerStateMachine playerStateMachine, PlayerValues playerValues, string animBoolName) : base(playerController, playerStateMachine, playerValues, animBoolName)
    {
    }

    private float currentRelativeVelocity;

    public override void Enter()
    {
        base.Enter();
        currentRelativeVelocity = playerController.m_rb2D.velocity.x/playerValues.moveMaxVelocity;
    }

    public override void Update()
    {
        base.Update();

        playerController.CheckIfShouldFlip(playerController.m_playerInputHandler.absoluteMovementInput);

        playerController.SetVelocityX(CalculateNewVelocity());

        if (playerController.CheckIfCanDash())
        {
            playerStateMachine.ChangeState(playerController.dashState);
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

}
