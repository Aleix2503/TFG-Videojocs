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
    public override void Update()
    {
        base.Update();
        if (_playerController.m_playerInputHandler.absoluteMovementInput != 0 && _playerPhysics.isGrounded)
        {
            _playerStateMachine.ChangeState(_playerController.moveState);
        }
        else if (_playerPhysics.rb2D.velocity.y < 0 && !_playerPhysics.isGrounded)
        {
            _playerStateMachine.ChangeState(_playerController.fallState);
        }
        else if (_playerController.m_playerInputHandler.jumpInput && _playerPhysics.isGrounded)
        {
            _playerStateMachine.ChangeState(_playerController.jumpState);
        }/*else if (_playerController.m_playerInputHandler.attackInput) 
        {
            _playerStateMachine.ChangeState(_playerController.attackState);
        }*/
        else if (_playerController.m_playerInputHandler.dashInput || _playerController.m_playerInputHandler.bubbleInput || _playerController.m_playerInputHandler.expandInput)
        {
            _playerStateMachine.ChangeState(_playerController.abilityState);
        }

    }
}
