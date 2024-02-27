using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawnState : PlayerState
{
    public PlayerRespawnState(PlayerController playerController, PlayerStateMachine playerStateMachine, PlayerValues playerValues, string animBoolName) : base(playerController, playerStateMachine, playerValues, animBoolName)
    {
    }

    public override void DoChecks()
    {
        
    }

    public override void Enter()
    {
        base.Enter();

        playerController.FadePlayerColor(playerValues.defaultColor, 0);

        playerController.FreezePlayerPosition(true);

        playerController.Respawn();
    }

    public override void Update()
    {
        if (Time.time > startTime + playerValues.respawnToIdleSeconds)
        {
            playerStateMachine.ChangeState(playerController.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();

        playerController.FreezePlayerPosition(false);
    }
}
