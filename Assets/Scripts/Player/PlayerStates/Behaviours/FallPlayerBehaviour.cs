using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallPlayerBehaviour : AirPlayerBehaviour
{
    private bool hasTouchedCeiling = false;
    private bool isTouchingCeiling = false;
    public FallPlayerBehaviour(PlayerStateMachine playerStateMachine, PlayerPhysics playerPhysics, PlayerController playerController) : base(playerStateMachine, playerPhysics, playerController)
    {
    }
    public override void Enter()
    {
        base.Enter();
        hasTouchedCeiling = false;
        isFalling = false;
    }
    public override void DoChecks()
    {
        base.DoChecks();
        isTouchingCeiling = _playerPhysics.checkIfTouchingCeiling();
    }
    private bool isFalling = false;
    public override void Logic()
    {
        base.Logic();
        if (isTouchingCeiling&&!hasTouchedCeiling)
        {
            PaintManager._instance.PlaceSplat(_playerPhysics.transform.position + new Vector3(0, 0.3f, 0), Vector3.down, _playerController.playerControlValues.defaultColor);
            hasTouchedCeiling = true;
        }
        _playerController.CheckCoyoteTime(startingTime);


        if (_playerPhysics.rb2D.velocity.y > 0) { _playerPhysics.BackToEarth(); }
        else { _playerPhysics.Fall(); }

        if (_playerPhysics.rb2D.velocity.y <= _playerPhysics.playerPhysicsValues.fallTerminalVelocity)
        {
            _playerPhysics.FallMaxSpeed();
        }


        if (_playerController.isCoyoteTimeActive && _playerController.m_playerInputHandler.jumpInput)
        {
            _playerController.isCoyoteTimeActive = false;
            _playerStateMachine.ChangeState(_playerController.jumpState);
            return;
        }
        else if (!_playerController.isCoyoteTimeActive&&!isFalling)
        {
            _playerStateMachine.SetAnim(3);
            isFalling = true;
        }

        if (_playerPhysics.isGrounded)
        {
            _playerStateMachine.SetAnim(1);
            PaintManager._instance.PlaceOnFallTrace();
            if (_playerController.m_playerInputHandler.absoluteMovementInput == 0)
            {
                _playerStateMachine.ChangeState(_playerController.idleState);
                return;
            }
            else
            {
                _playerStateMachine.ChangeState(_playerController.moveState);
                return;
            }
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
        if (_playerController.CheckIfCanExpand())
        {
            _playerStateMachine.ChangeState(_playerController.expandState);
            return;
        }

    }
}
