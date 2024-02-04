using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerState
{
    public PlayerMoveState(PlayerController playerController, PlayerStateMachine playerStateMachine, PlayerValues playerValues, string animBoolName) : base(playerController, playerStateMachine, playerValues, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();

        playerController.SetVelocityX(playerValues.moveSpeed * playerController.m_playerInputHandler.movementInput);

        if (playerController.m_playerInputHandler.jumpInput == true)
        {
            playerStateMachine.ChangeState(playerController.jumpState);
        }

        if (playerController.m_playerInputHandler.movementInput == 0)
        {
            playerStateMachine.ChangeState(playerController.idleState);
        }

        if (!isGrounded)
        {
            playerStateMachine.ChangeState(playerController.fallState);
        }
    }
}
