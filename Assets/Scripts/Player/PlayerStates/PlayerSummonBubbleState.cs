using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSummonBubbleState : PlayerState
{
    public PlayerSummonBubbleState(PlayerController playerController, PlayerStateMachine playerStateMachine, PlayerValues playerValues, string animBoolName) : base(playerController, playerStateMachine, playerValues, animBoolName)
    {
    }
    private bool isBubbleInstantiated;

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        playerController.SetVelocityX(0);
        isBubbleInstantiated = false;

        playerController.FadePlayerColor(playerValues.bubbleColor, playerValues.bubbleSummonColorFadeInTime);
    }

    public override void Exit()
    {
        base.Exit();

        playerController.FadePlayerColor(playerValues.defaultColor, playerValues.bubbleSummonColorFadeOutTime);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();

        if (Time.time > startTime + playerValues.bubbleSummonDelay && !isBubbleInstantiated)
        {
            playerController.InstantiateBubble();
            isBubbleInstantiated = true;
        } else if (Time.time > startTime + playerValues.bubbleSummonFinishTime)
        {
            playerStateMachine.ChangeState(playerController.idleState);
        }
    }
}
