using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExpandedState : PlayerState
{
    public PlayerExpandedState(PlayerController playerController, PlayerStateMachine playerStateMachine, PlayerValues playerValues, string animBoolName) : base(playerController, playerStateMachine, playerValues, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        playerController.SetGravityScale(playerValues.defaultGravity * playerValues.expandGravityMultiplier);
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
        if (Time.time > startTime + playerValues.expandMaxFallTime)
        {
            playerStateMachine.ChangeState(playerController.endExpandState);
        }
        if (isGrounded) //TODO grounded logic with expanded collision?
        {
            playerStateMachine.ChangeState(playerController.endExpandState);
        }

    }
}
