using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*public class PlayerNoMoveFallState : PlayerAirState
{
    public PlayerNoMoveFallState(PlayerController playerController, PlayerStateMachine playerStateMachine, PlayerValues playerValues, string animBoolName) : base(playerController, playerStateMachine, playerValues, animBoolName)
    {
    }

    public float secondsLeft = 0;

    public override void Enter()
    {
        base.Enter();
        playerController.SetVelocityX(0);
    }

    public override void Update()
    {
        if (playerController.m_rb2D.velocity.y > 0)
        {
            playerController.m_rb2D.AddForce(new Vector2(0, playerValues.fallForceWhenGoingUp));
            if (playerController.m_rb2D.velocity.y < 0)
            {
                playerController.SetVelocityY(0);
            }
        }
        else
        {
            playerController.m_rb2D.AddForce(new Vector2(0, playerValues.fallForce));
        }

        if (playerController.m_rb2D.velocity.y <= playerValues.fallTerminalVelocity)
        {
            playerController.SetVelocityY(playerValues.fallTerminalVelocity);
        }

        if (isGrounded)
        {
            PaintManager._instance.PlaceOnFallTrace();

            playerController.noMoveIdleState.secondsLeft = secondsLeft;
            playerStateMachine.ChangeState(playerController.noMoveIdleState);
        }

        secondsLeft -= Time.deltaTime;

        if (secondsLeft <= 0)
        {
            playerStateMachine.ChangeState(playerController.fallState);
        }
    }

    
}*/
