using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerAirState
{
    public PlayerJumpState(PlayerController playerController, PlayerStateMachine playerStateMachine, PlayerValues playerValues, string animBoolName) : base(playerController, playerStateMachine, playerValues, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        playerController.SetVelocityY(playerValues.jumpVelocity);
    }


    public override void Update()
    {
        base.Update();

        if (playerValues.jumpCanPlayerFlip)
        {
            playerController.CheckIfShouldFlip(playerController.m_playerInputHandler.absoluteMovementInput);
        }

        if (playerController.m_playerInputHandler.dashInput == true)
        {
            playerController.m_playerInputHandler.UseDashInput();
            playerStateMachine.ChangeState(playerController.dashState);
        }

        if (!playerController.m_playerInputHandler.jumpInputHeld || playerController.m_rb2D.velocity.y <= 0)
        {
            playerStateMachine.ChangeState(playerController.fallState);
        }
    }
}
