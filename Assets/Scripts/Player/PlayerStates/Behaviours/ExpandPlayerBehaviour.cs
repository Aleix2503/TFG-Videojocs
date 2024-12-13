using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandPlayerBehaviour : PlayerBehaviour
{
    private bool expandedIsTouchingHazard;

    public bool canBreakGround;
    private float transitionTime;
    public ExpandPlayerBehaviour(PlayerStateMachine playerStateMachine, PlayerPhysics playerPhysics, PlayerController playerController) : base(playerStateMachine, playerPhysics, playerController)
    {
    }
    public override void DoChecks()
    {
        base.DoChecks();
        expandedIsTouchingHazard = _playerPhysics.checkIfExpandedTouchingHazard();
    }

    public override void Enter()
    {
        base.Enter();
        _playerController.ExpandIn();
        _playerPhysics.ExpandIn();
        canBreakGround = false;
        transitionTime = _playerPhysics.playerPhysicsValues.expandTime;
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
        transitionTime -= Time.deltaTime;
        if ((_playerPhysics.checkIfExpandedTouchingGround() || _playerPhysics.checkIfExpandedCollision()||_playerPhysics.checkIfGrounded())&&transitionTime<=0)
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
