using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdlePlayerBehaviour : PlayerBehaviour
{
    public IdlePlayerBehaviour(PlayerStateMachine playerStateMachine, PlayerPhysics playerPhysics, PlayerController playerController) : base(playerStateMachine, playerPhysics, playerController) { }

    public override void Enter()
    {
        base.Enter();
        _playerPhysics.Stop();
    }
    public override void Logic()
    {
        base.Logic();
        if (_playerController.CheckIfCanDash())
        {
            _playerStateMachine.ChangeState(_playerController.dashState);
        }
        else if (_playerController.CheckIfCanBubble())
        {
            _playerStateMachine.ChangeState(_playerController.bubbleState);
        }
        else if (_playerController.CheckIfCanExpand())
        {
            _playerStateMachine.ChangeState(_playerController.expandState);
        }
        else
        {
            if (_playerController.m_playerInputHandler.jumpInput == true)
            {
                _playerStateMachine.ChangeState(_playerController.jumpState);
            }

            if (_playerController.m_playerInputHandler.absoluteMovementInput != 0)
            {
                _playerStateMachine.ChangeState(_playerController.moveState);
            }
            if (!_playerPhysics.isGrounded)
            {
                _playerController.StartCoyoteTime();
                _playerStateMachine.ChangeState(_playerController.fallState);
            }
        }
    }
}
