using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerState
{
    public PlayerMoveState(PlayerController playerController, PlayerStateMachine playerStateMachine, PlayerValues playerValues, string animBoolName) : base(playerController, playerStateMachine, playerValues, animBoolName)
    {
    }

    private float currentRelativeVelocity;

    public override void Enter()
    {
        base.Enter();

        currentRelativeVelocity = playerController.m_rb2D.velocity.x/playerValues.moveAccelerationSeconds;
    }

    public override void Update()
    {
        base.Update();

        playerController.CheckIfShouldFlip(playerController.m_playerInputHandler.absoluteMovementInput);

        playerController.SetVelocityX(CalculateNewVelocity());

        if (playerController.m_playerInputHandler.jumpInput == true)
        {
            playerStateMachine.ChangeState(playerController.jumpState);
        }

        if (playerController.m_playerInputHandler.absoluteMovementInput == 0)
        {
            playerStateMachine.ChangeState(playerController.idleState);
        }

        if (!isGrounded)
        {
            playerStateMachine.ChangeState(playerController.fallState);
        }
    }

    private float CalculateNewVelocity()
    {
        float targetVelocity = playerValues.moveMaxVelocity * playerController.m_playerInputHandler.absoluteMovementInput;

        float accelerationRate = playerValues.moveMaxVelocity / playerValues.moveAccelerationSeconds;

        currentRelativeVelocity += accelerationRate * Time.deltaTime * Mathf.Sign(targetVelocity - currentRelativeVelocity);

        if (playerController.m_playerInputHandler.absoluteMovementInput > 0)
        {
            currentRelativeVelocity = Mathf.Min(currentRelativeVelocity, targetVelocity);
        }
        else if (playerController.m_playerInputHandler.absoluteMovementInput < 0)
        {
            currentRelativeVelocity = Mathf.Max(currentRelativeVelocity, targetVelocity);
        }

        return currentRelativeVelocity;
    }

}
