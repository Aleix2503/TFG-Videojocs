using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStartExpandState : PlayerState
{
    public PlayerStartExpandState(PlayerController playerController, PlayerStateMachine playerStateMachine, PlayerValues playerValues, string animBoolName) : base(playerController, playerStateMachine, playerValues, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        playerController.SetVelocityX(0);
        playerController.SetVelocityY(0);
        playerController.SetGravityScale(0);
    }

    public override void Exit()
    {
        base.Exit();
        playerController.SetGravityScale(playerValues.defaultGravity);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();

        if (Time.time > startTime + playerValues.startExpandTime)
        {
            playerStateMachine.ChangeState(playerController.expandedState);
        }
    }
}
