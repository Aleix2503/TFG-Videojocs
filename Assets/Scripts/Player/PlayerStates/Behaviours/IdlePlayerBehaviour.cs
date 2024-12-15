using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdlePlayerBehaviour : PlayerBehaviour
{
    public bool isInkstinctActive;
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
        if(Time.time> startingTime + _playerController.playerControlValues.inkstinctTime&&!isInkstinctActive)
        {
            _playerController.StartInkstinct();
            isInkstinctActive = true;
        }
        if (_playerController.CheckIfCanDash())
        {
            _playerController.StopInkstinct();
            isInkstinctActive = false;
            _playerStateMachine.ChangeState(_playerController.dashState);
            return;
        }
        if (_playerController.CheckIfCanBubble())
        {
            _playerController.StopInkstinct();
            isInkstinctActive = false;
            _playerStateMachine.ChangeState(_playerController.bubbleState);
            return;
        }
        if (_playerController.CheckIfCanCube())
        {
            _playerController.StopInkstinct();
            isInkstinctActive = false;
            _playerStateMachine.ChangeState(_playerController.cubeState);
            return;
        }
        if (_playerController.CheckIfCanJump())
        {
            _playerController.StopInkstinct();
            isInkstinctActive = false;
            _playerStateMachine.ChangeState(_playerController.jumpState);
            return;
        }

        if (_playerController.m_playerInputHandler.absoluteMovementInput != 0)
        {
            _playerController.StopInkstinct();
            isInkstinctActive = false;
            _playerStateMachine.ChangeState(_playerController.moveState);
            return;
        }
        if (!_playerPhysics.isGrounded)
        {
            if(_playerController.didPlayerTouchGroundSinceLastJump)_playerController.StartCoyoteTime();
            _playerController.StopInkstinct();
            isInkstinctActive = false;
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
