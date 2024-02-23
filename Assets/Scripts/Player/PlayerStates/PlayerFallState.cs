using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : PlayerAirState
{

    public PlayerFallState(PlayerController playerController, PlayerStateMachine playerStateMachine, PlayerValues playerValues, string animBoolName) : base(playerController, playerStateMachine, playerValues, animBoolName)
    {
    }

    private bool isCoyoteTimeActive;

    public override void Update()
    {
        base.Update();

        CheckCoyoteTime();

        if (playerValues.fallCanPlayerFlip)
        {
            playerController.CheckIfShouldFlip(playerController.m_playerInputHandler.absoluteMovementInput);
        }

        if (playerController.CheckIfCanDash())
        {
            playerStateMachine.ChangeState(playerController.dashState);
            return;
        }

        if (playerController.CheckIfCanExpand())
        {
            playerStateMachine.ChangeState(playerController.startExpandState);
            return;
        }

        if (playerController.m_rb2D.velocity.y > 0)
        {
            playerController.m_rb2D.AddForce(new Vector2(0, playerValues.fallForceWhenGoingUp));
            if (playerController.m_rb2D.velocity.y < 0)
            {
                playerController.SetVelocityY(0);
            }
        } else
        {
            playerController.m_rb2D.AddForce(new Vector2(0, playerValues.fallForce));
        }

        if (playerController.m_rb2D.velocity.y <= playerValues.fallTerminalVelocity)
        {
            playerController.SetVelocityY(playerValues.fallTerminalVelocity);
        }

        if (isCoyoteTimeActive && playerController.m_playerInputHandler.jumpInput)
        {
            isCoyoteTimeActive = false;
            playerController.m_playerInputHandler.UseJumpInput();
            playerStateMachine.ChangeState(playerController.jumpState);
        }
        
        if (isGrounded)
        {
            PaintManager._instance.PlaceOnFallTrace();
            if (playerController.m_playerInputHandler.absoluteMovementInput == 0)
            {
                playerStateMachine.ChangeState(playerController.idleState);
            } else
            {
                playerStateMachine.ChangeState(playerController.moveState);
            }
        }
    }

    private void CheckCoyoteTime()
    {
        if (isCoyoteTimeActive && Time.time > startTime + playerValues.coyoteTime)
        {
            isCoyoteTimeActive = false;
        }
    }

    public void StartCoyoteTime() => isCoyoteTimeActive = true;
}
