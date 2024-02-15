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
        playerController.FreezePlayerPosition(true);
    }

    public override void Exit()
    {
        base.Exit();
        playerController.SetGravityScale(playerValues.defaultGravity);
        playerController.FreezePlayerPosition(false);
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
