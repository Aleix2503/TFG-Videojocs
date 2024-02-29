using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(PlayerController playerController, PlayerStateMachine playerStateMachine, PlayerValues playerValues, string animBoolName) : base(playerController, playerStateMachine, playerValues, animBoolName)
    {
    }

    private float currentRelativeVelocity;

    private bool isTouchingBubble;

    private bool isTouchingFrontWall = false;

    private float lastWallPaintTime = 0;

    public override void Enter()
    {
        base.Enter();
        currentRelativeVelocity = playerController.m_rb2D.velocity.x / playerValues.airMoveMaxVelocity;
    }

    public override void Update()
    {
        base.Update();

        if (isTouchingFrontWall)
        {
            TryPaintFrontWall();
        }

        if (isTouchingBubble)
        {
            playerStateMachine.ChangeState(playerController.bubbledState);
            return;
        }

        playerController.SetVelocityX(CalculateNewVelocity());
    }

    private float CalculateNewVelocity()
    {
        int movementInput = playerController.m_playerInputHandler.absoluteMovementInput;

        if (currentRelativeVelocity * movementInput < 0 || movementInput == 0)
        {
            currentRelativeVelocity = 0;
        }
        else
        {
            currentRelativeVelocity += (Time.deltaTime / playerValues.airMoveAccelerationSeconds) * movementInput;
        }

        currentRelativeVelocity = Mathf.Clamp(currentRelativeVelocity, -1, 1);

        return playerValues.airMoveMaxVelocity * currentRelativeVelocity;
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isTouchingBubble = playerController.checkIfTouchingBubble();
        isTouchingFrontWall = playerController.checkIfTouchingFrontWall();
    }

    private void TryPaintFrontWall()
    {
        if (Time.time > lastWallPaintTime + 0.2f)
        {
            PaintManager._instance.PlaceSplat(playerController.transform.position + new Vector3(0.5f * playerController.facingDirection, 0, 0),
                Vector3.left * playerController.facingDirection, playerValues.defaultColor);

            lastWallPaintTime = Time.time;
        }
    }
}
