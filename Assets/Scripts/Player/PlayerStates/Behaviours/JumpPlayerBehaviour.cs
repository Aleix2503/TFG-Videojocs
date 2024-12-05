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
        Debug.Log("Jump");
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
        if (!_playerController.m_playerInputHandler.jumpInputHeld||_playerPhysics.rb2D.velocity.y<=0)
        {
            _playerStateMachine.ChangeState(_playerController.fallState);
        }
    }

}
