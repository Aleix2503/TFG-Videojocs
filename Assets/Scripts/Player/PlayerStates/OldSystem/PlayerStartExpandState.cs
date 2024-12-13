using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*public class PlayerStartExpandState : PlayerState
{
    public PlayerStartExpandState(PlayerController playerController, PlayerStateMachine playerStateMachine, PlayerValues playerValues, string animBoolName) : base(playerController, playerStateMachine, playerValues, animBoolName)
    {
    }

    bool isInsidePlatform = false;

    public override void DoChecks()
    {
        base.DoChecks();
        isInsidePlatform = playerController.checkIfExpandedCollision();
    }

    public override void Enter()
    {
        base.Enter();
        playerController.FreezePlayerPosition(true);

        playerController.FadePlayerColor(playerValues.expandColor, playerValues.startExpandColorFadeInTime);
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
            if (isInsidePlatform)
            {
                playerStateMachine.ChangeState(playerController.endExpandState);

            } else
            {
                playerStateMachine.ChangeState(playerController.expandedState);
            }
        }
    }
}*/
