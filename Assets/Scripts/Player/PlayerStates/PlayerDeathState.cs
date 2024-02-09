using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathState : PlayerState
{
    public PlayerDeathState(PlayerController playerController, PlayerStateMachine playerStateMachine, PlayerValues playerValues, string animBoolName) : base(playerController, playerStateMachine, playerValues, animBoolName)
    {
    }

    public override void DoChecks()
    {

    }

    public override void Enter()
    {
        base.Enter();

        playerController.SetVelocityX(0);
        playerController.SetVelocityY(0);
    }

    public override void Update()
    {
        if (Time.time > startTime + playerValues.deathToRespawnSeconds)
        {
            playerStateMachine.ChangeState(playerController.respawnState);
        }
    }
}
