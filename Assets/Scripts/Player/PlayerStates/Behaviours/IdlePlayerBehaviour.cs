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
        PaintManager._instance.PlaceSplat(_playerPhysics.transform.position + new Vector3(0, -0.5f, 0),
                Vector3.up, _playerController.currentPlayerColor);
    }
    public override void Logic()
    {
        base.Logic();
        if (_playerController.CheckIfCanDash())
        {
            _playerStateMachine.ChangeState(_playerController.dashState);
            return;
        }
        if (_playerController.CheckIfCanBubble())
        {
            _playerStateMachine.ChangeState(_playerController.bubbleState);
            return;
        }
        if (_playerController.CheckIfCanExpand())
        {
            _playerStateMachine.ChangeState(_playerController.expandState);
            return;
        }
        if (_playerController.CheckIfCanJump())
        {
            _playerStateMachine.ChangeState(_playerController.jumpState);
            return;
        }

        if (_playerController.m_playerInputHandler.absoluteMovementInput != 0)
        {
            _playerStateMachine.ChangeState(_playerController.moveState);
            return;
        }
        if (!_playerPhysics.isGrounded)
        {
            if(_playerController.didPlayerTouchGroundSinceLastJump)_playerController.StartCoyoteTime();
            _playerStateMachine.ChangeState(_playerController.fallState);
            return;
        }
    }
    public override void Physics()
    {
        base.Physics();
        if (_playerPhysics.isGrounded)
        {
            _playerController.ResetGroundFlags();
        }
    }
}
