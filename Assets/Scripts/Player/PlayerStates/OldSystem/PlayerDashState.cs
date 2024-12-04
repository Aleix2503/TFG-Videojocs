using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(PlayerController playerController, PlayerStateMachine playerStateMachine, PlayerValues playerValues, string animBoolName) : base(playerController, playerStateMachine, playerValues, animBoolName)
    {
    }
    bool isTouchingFrontWall;
    public override void DoChecks()
    {
        base.DoChecks();
        isTouchingFrontWall = playerController.checkIfTouchingFrontWall();
    }

    public override void Enter()
    {
        base.Enter();
        playerController.SetVelocityY(0);
        playerController.SetVelocityX(playerValues.dashVelocity * playerController.facingDirection);
        playerController.SetGravityScale(0);
        playerController.SetLinearDrag(playerValues.dashLinearDrag);

        isTouchingFrontWall = false;

        playerController.FadePlayerColor(playerValues.dashColor, playerValues.dashColorFadeInTime);
        
        PaintManager._instance.EmitDashParticles();
        playerController.SetPaintingState(PlayerPaintingState.dashing);
    }

    public override void Exit()
    {
        base.Exit();
        playerController.SetGravityScale(playerValues.defaultGravity);
        playerController.SetLinearDrag(playerValues.defaultLinearDrag);

        playerController.FadePlayerColor(playerValues.defaultColor, playerValues.dashColorFadeOutTime);

        playerController.SetPaintingState(PlayerPaintingState.def);
    }

    public override void Update()
    {
        base.Update();

        playerController.SetVelocityY(0);

        float elapsedTime = Time.time - startTime;
        float dashDuration = playerValues.dashTime;

        float fraction = elapsedTime / dashDuration;

        float currentDrag = Mathf.Lerp(playerValues.dashLinearDrag, playerValues.defaultLinearDrag, fraction);
        playerController.SetLinearDrag(currentDrag);

        if (elapsedTime > dashDuration)
        {
            playerStateMachine.ChangeState(playerController.fallState);
        }

        if (isTouchingFrontWall)
        {
            PaintManager._instance.PlaceSplat(playerController.transform.position + new Vector3(0.5f * playerController.facingDirection, 0, 0),
                Vector3.left * playerController.facingDirection, playerValues.dashColor);
            playerStateMachine.ChangeState(playerController.fallState);
        }
    }
}
