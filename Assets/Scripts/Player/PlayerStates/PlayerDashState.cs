using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(PlayerController playerController, PlayerStateMachine playerStateMachine, PlayerValues playerValues, string animBoolName) : base(playerController, playerStateMachine, playerValues, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        playerController.SetVelocityY(0);
        playerController.SetVelocityX(playerValues.dashVelocity * playerController.facingDirection);
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
        if (Time.time > startTime + playerValues.dashTime)
        {
            playerStateMachine.ChangeState(playerController.fallState);
        }
    }
}
