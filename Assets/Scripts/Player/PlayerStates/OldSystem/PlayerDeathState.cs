using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*public class PlayerDeathState : PlayerState
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

        PaintManager._instance.PlaceSplat(playerController.transform.position + new Vector3(0, -0.5f, 0),
                Vector3.up, playerController.currentPlayerColor);
        PaintManager._instance.PlaceSplat(playerController.transform.position + new Vector3(0.5f, 0, 0),
                Vector3.left, playerController.currentPlayerColor);
        PaintManager._instance.PlaceSplat(playerController.transform.position + new Vector3(0, 0.5f, 0),
                Vector3.down, playerController.currentPlayerColor);
        PaintManager._instance.PlaceSplat(playerController.transform.position + new Vector3(-0.5f, 0, 0),
            Vector3.right, playerController.currentPlayerColor);

        playerController.FreezePlayerPosition(true);
    }

    public override void Update()
    {
        if (Time.time > startTime + playerValues.deathToRespawnSeconds)
        {
            playerStateMachine.ChangeState(playerController.respawnState);
        }
    }
}*/
