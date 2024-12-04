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
        if (_playerController.CheckIfCanDash())
        {
            //Add bubble and expansion
            _playerStateMachine.ChangeState(_playerController.abilityState);
        }
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
