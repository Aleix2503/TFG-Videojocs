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

    public override void Enter()
    {
        base.Enter();
        currentRelativeVelocity = playerController.m_rb2D.velocity.x / playerValues.airMoveMaxVelocity;
    }

    public override void Update()
    {
        base.Update();

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
    }
}
