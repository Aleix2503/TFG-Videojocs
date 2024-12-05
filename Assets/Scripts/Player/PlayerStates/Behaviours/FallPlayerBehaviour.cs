using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallPlayerBehaviour : AirPlayerBehaviour
{
    public FallPlayerBehaviour(PlayerStateMachine playerStateMachine, PlayerPhysics playerPhysics, PlayerController playerController) : base(playerStateMachine, playerPhysics, playerController)
    {
    }
    public override void Update()
    {
        _playerController.CheckCoyoteTime(startingTime);

        if (_playerController.CheckIfCanDash() || _playerController.CheckIfCanBubble() || _playerController.CheckIfCanExpand())
        {
            _playerStateMachine.ChangeState(_playerController.abilityState);
        }

        if (_playerPhysics.rb2D.velocity.y > 0) {_playerPhysics.BackToEarth();}
        else {_playerPhysics.Fall();}

        if (_playerPhysics.rb2D.velocity.y <= _playerPhysics.playerPhysicsValues.fallTerminalVelocity)
        {
            _playerPhysics.FallMaxSpeed();
        }
        if (_playerController.isCoyoteTimeActive && _playerController.m_playerInputHandler.jumpInput)
        {
            _playerController.isCoyoteTimeActive = false;
            _playerStateMachine.ChangeState(_playerController.jumpState);
        }
        if (_playerPhysics.isGrounded)
        {
            if (_playerController.m_playerInputHandler.absoluteMovementInput == 0)
            {
                _playerStateMachine.ChangeState(_playerController.idleState);
            }
            else
            {
                _playerStateMachine.ChangeState(_playerController.moveState);
            }
        }

    }
}
