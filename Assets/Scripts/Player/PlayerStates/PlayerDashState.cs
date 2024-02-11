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
        playerController.SetLinearDrag(playerValues.dashLinearDrag);
    }

    public override void Exit()
    {
        base.Exit();
        playerController.SetGravityScale(playerValues.defaultGravity);
        playerController.SetLinearDrag(playerValues.defaultLinearDrag);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();
        float elapsedTime = Time.time - startTime;
        float dashDuration = playerValues.dashTime;

        if (elapsedTime < dashDuration)
        {
            float fraction = elapsedTime / dashDuration;

            float currentDrag = Mathf.Lerp(playerValues.dashLinearDrag, playerValues.defaultLinearDrag, fraction);
            playerController.SetLinearDrag(currentDrag);
        }
        else
        {
            playerStateMachine.ChangeState(playerController.fallState);
        }
    }
}
