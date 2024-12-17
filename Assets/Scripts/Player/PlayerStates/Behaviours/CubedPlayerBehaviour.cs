using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubedPlayerBehaviour : PlayerBehaviour
{
    private bool cubedIsTouchingHazard;

    public bool canBreakGround;
    private float transitionTime;
    public CubedPlayerBehaviour(PlayerStateMachine playerStateMachine, PlayerPhysics playerPhysics, PlayerController playerController) : base(playerStateMachine, playerPhysics, playerController)
    {
    }
    public override void DoChecks()
    {
        base.DoChecks();
        cubedIsTouchingHazard = _playerPhysics.checkIfCubedTouchingHazard();
    }

    public override void Enter()
    {
        base.Enter();
        _playerController.CubeIn();
        _playerPhysics.CubeIn();
        canBreakGround = false;
        transitionTime = _playerPhysics.playerPhysicsValues.cubedTime;
    }
    public void Exit()
    {
        _playerController.CubeOut();
        _playerPhysics.CubeOut();
        _playerStateMachine.ChangeState(_playerController.idleState);
    }
    public override void Logic()
    {
        base.Logic();
        _playerPhysics.CubedFall();
        transitionTime -= Time.deltaTime;
        if ((_playerPhysics.checkIfCubedTouchingGround() || _playerPhysics.checkIfCubedCollision()||_playerPhysics.checkIfGrounded())&&transitionTime<=0)
        {
            PaintManager._instance.EmitCubedParticles();
            PaintManager._instance.PlaceOnCubedTrace();
            _playerStateMachine.SetSound("cube");
            Exit();
        }
    }
    public override void Physics()
    {
        base.Physics();
        _playerPhysics.SetVelocityX(0);
        if (_playerPhysics.rb2D.velocity.y < _playerController.playerControlValues.cubedBreakPlatformVelocityThreshold)
        {
            canBreakGround = true;
        }
    }

}
