using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPlayerBehaviour : AirPlayerBehaviour
{
    public JumpPlayerBehaviour(PlayerStateMachine playerStateMachine, PlayerPhysics playerPhysics, PlayerController playerController) : base(playerStateMachine, playerPhysics, playerController)
    {
    }
    public override void Enter()
    {
        base.Enter();
        _playerPhysics.Jump();
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
        else if (!_playerController.m_playerInputHandler.jumpInputHeld||_playerPhysics.rb2D.velocity.y<=0)
        {
            _playerStateMachine.ChangeState(_playerController.fallState);
        }
    }

}
