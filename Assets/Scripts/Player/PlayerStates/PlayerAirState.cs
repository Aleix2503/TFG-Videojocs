using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(PlayerController playerController, PlayerStateMachine playerStateMachine, PlayerValues playerValues, string animBoolName) : base(playerController, playerStateMachine, playerValues, animBoolName)
    {
    }

    private float currentRelativeVelocity;

    public override void Enter()
    {
        base.Enter();

        currentRelativeVelocity = playerController.m_rb2D.velocity.x / playerValues.airMoveAccelerationSeconds;
    }

    public override void Update()
    {
        base.Update();

        playerController.SetVelocityX(CalculateNewVelocity());
    }

    private float CalculateNewVelocity()
    {
        float targetVelocity = playerValues.moveMaxVelocity * playerController.m_playerInputHandler.absoluteMovementInput;

        float acceleration = (targetVelocity - currentRelativeVelocity) / playerValues.moveAccelerationSeconds;

        if (playerController.m_playerInputHandler.absoluteMovementInput != 0)
        {
            currentRelativeVelocity += acceleration * Time.deltaTime;
            currentRelativeVelocity = Mathf.Clamp(currentRelativeVelocity, -playerValues.moveMaxVelocity, playerValues.moveMaxVelocity);
        }
        else
        {
            currentRelativeVelocity = 0;
        }

        return currentRelativeVelocity;
    }

}
