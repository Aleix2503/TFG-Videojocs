using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandPlayerBehaviour : PlayerBehaviour
{
    private bool isInsidePlatform;
    private bool expandedIsGrounded;
    private bool expandedIsTouchingHazard;

    private bool canBreakGround;
    public ExpandPlayerBehaviour(PlayerStateMachine playerStateMachine, PlayerPhysics playerPhysics, PlayerController playerController) : base(playerStateMachine, playerPhysics, playerController)
    {
    }
    public override void DoChecks()
    {
        base.DoChecks();
        isInsidePlatform = _playerPhysics.checkIfExpandedCollision();
        expandedIsTouchingHazard = _playerPhysics.checkIfExpandedTouchingHazard();
        expandedIsGrounded = _playerPhysics.checkIfExpandedTouchingGround();
    }

    public override void Enter()
    {
        base.Enter();
        _playerController.ExpandIn();
        _playerPhysics.ExpandIn();
        canBreakGround = false;
    }
    public void Exit()
    {
        _playerController.ExpandOut();
        _playerPhysics.ExpandOut();
        _playerStateMachine.ChangeState(_playerController.idleState);
    }
    public override void Logic()
    {
        base.Logic();
        _playerPhysics.ExpandedFall();
        if (expandedIsGrounded || isInsidePlatform)
        {
            PaintManager._instance.EmitExpandedParticles();
            PaintManager._instance.PlaceOnExpandedTrace();
            Exit();
        }
    }
    public override void Physics()
    {
        base.Physics();
        _playerPhysics.SetVelocityX(0);
        if (_playerPhysics.rb2D.velocity.y < _playerController.playerControlValues.expandBreakPlatformVelocityThreshold)
        {
            canBreakGround = true;
        }
    }

}
