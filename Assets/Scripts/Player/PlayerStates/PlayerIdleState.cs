using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(PlayerController playerController, PlayerStateMachine playerStateMachine, PlayerValues playerValues, string animBoolName) : base(playerController, playerStateMachine, playerValues, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        playerController.ResetGroundFlags();
        playerController.SetVelocityX(0);

        PaintManager._instance.PlaceOnFallTrace();
    }

    public override void Update()
    {
        base.Update();

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

        if (playerController.m_playerInputHandler.absoluteMovementInput != 0)
        {
            playerStateMachine.ChangeState(playerController.moveState);
        }

        if (!isGrounded)
        {
            playerController.fallState.StartCoyoteTime();
            playerStateMachine.ChangeState(playerController.fallState);
        }
    }
}
