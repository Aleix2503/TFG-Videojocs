using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNoMoveIdleState : PlayerState
{
    public PlayerNoMoveIdleState(PlayerController playerController, PlayerStateMachine playerStateMachine, PlayerValues playerValues, string animBoolName) : base(playerController, playerStateMachine, playerValues, animBoolName)
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
        base.Update();

        if (!isGrounded)
        {
            playerController.noMoveFallState.secondsLeft = secondsLeft;
            playerStateMachine.ChangeState(playerController.noMoveFallState);
        }

        secondsLeft -= Time.deltaTime;

        if (secondsLeft <= 0)
        {
            playerStateMachine.ChangeState(playerController.idleState);
        }
    }
}
