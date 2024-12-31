using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayerBehaviour : FloorPlayerBehaviour
{
    private bool isTouchingFrontWall = false;
    private bool hasTouchedFrontWall = false;
    public MovePlayerBehaviour(PlayerStateMachine playerStateMachine, PlayerPhysics playerPhysics, PlayerController playerController) : base(playerStateMachine, playerPhysics, playerController) { }

    public override void Enter()
    {
        base.Enter();
        _playerController.SetPaintingState(PlayerPaintingState.moving);
        hasTouchedFrontWall = false;
    }
    public override void Logic()
    {
        base.Logic();
        if (isTouchingFrontWall && !hasTouchedFrontWall)
        {
            PaintManager._instance.PlaceSplat(_playerController.transform.position + new Vector3(0.5f * _playerPhysics.facingDirection, 0, 0),
                Vector3.left * _playerPhysics.facingDirection, _playerController.playerControlValues.defaultColor);

            hasTouchedFrontWall = true;
        }
        if (!_playerPhysics.isGrounded)
        {
            if (_playerController.didPlayerTouchGroundSinceLastJump) _playerController.StartCoyoteTime();
            _playerStateMachine.ChangeState(_playerController.fallState);
            return;
        }
        if (_playerController.CheckIfCanJump())
        {
            _playerStateMachine.ChangeState(_playerController.jumpState);
            return;
        }
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
        if (_playerController.CheckIfCanCube())
        {
            _playerStateMachine.ChangeState(_playerController.cubeState);
            return;
        }
        if (_playerController.m_playerInputHandler.absoluteMovementInput == 0)
        {
            _playerStateMachine.ChangeState(_playerController.idleState);
            return;
        }


    }
    public override void Physics()
    {
        base.Physics();
        
    }
    public override void DoChecks()
    {
        base.DoChecks();
        isTouchingFrontWall = _playerPhysics.checkIfTouchingFrontWall();
    }
}
