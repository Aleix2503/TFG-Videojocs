using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayerBehaviour : FloorPlayerBehaviour
{
    
    public MovePlayerBehaviour(PlayerStateMachine playerStateMachine, PlayerPhysics playerPhysics, PlayerController playerController) : base(playerStateMachine, playerPhysics, playerController) { }

    public override void Enter()
    {
        base.Enter();

    }
    public override void Logic()
    {
        base.Logic();
        if (_playerController.CheckIfCanDash())
        {
            _playerStateMachine.ChangeState(_playerController.dashState);
        }
        if (_playerController.CheckIfCanBubble())
        {
            _playerStateMachine.ChangeState(_playerController.bubbleState);
        }
        if (_playerController.CheckIfCanExpand())
        {
            _playerStateMachine.ChangeState(_playerController.expandState);
        }
        if (_playerController.m_playerInputHandler.jumpInput == true)
        {
            _playerStateMachine.ChangeState(_playerController.jumpState);
        }
        if(_playerController.m_playerInputHandler.absoluteMovementInput == 0)
        {
            _playerStateMachine.ChangeState(_playerController.idleState);
        }
        if(!_playerPhysics.isGrounded)
        {
            _playerController.StartCoyoteTime();
            _playerStateMachine.ChangeState(_playerController.fallState);
        }
    }
    public override void Physics()
    {
        base.Physics();
        
    }
}
