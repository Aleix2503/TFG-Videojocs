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
        playerController.SetVelocityX(0);
    }

    public override void Update()
    {
        base.Update();
        if (playerController.m_playerInputHandler.jumpInput == true)
        {
            playerStateMachine.ChangeState(playerController.jumpState);
        }


        if (playerController.m_playerInputHandler.absoluteMovementInput != 0)
        {
            playerStateMachine.ChangeState(playerController.moveState);
        }

        if (!isGrounded)
        {
            playerStateMachine.ChangeState(playerController.fallState);
        }
    }
}
