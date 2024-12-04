using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEndExpandState : PlayerState
{
    public PlayerEndExpandState(PlayerController playerController, PlayerStateMachine playerStateMachine, PlayerValues playerValues, string animBoolName) : base(playerController, playerStateMachine, playerValues, animBoolName)
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
        playerController.FreezePlayerPosition(false);
        playerController.FadePlayerColor(playerValues.defaultColor, playerValues.endExpandColorFadeOutTime);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();
        if (Time.time > startTime + playerValues.endExpandTime)
        {
            if (!isGrounded)
            {
                playerStateMachine.ChangeState(playerController.fallState);
            } else
            {
                playerStateMachine.ChangeState(playerController.idleState);
            }
        }
    }
}
