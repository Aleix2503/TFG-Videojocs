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

        if (playerController.m_rb2D.velocity.y > 0)
        {
            playerController.m_rb2D.AddForce(new Vector2(0, playerValues.fallForceWhenGoingUp));
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
            playerStateMachine.ChangeState(playerController.jumpState);
        }
        
        if (isGrounded)
        {
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
