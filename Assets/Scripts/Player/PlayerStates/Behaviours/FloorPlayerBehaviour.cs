using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorPlayerBehaviour : PlayerBehaviour
{
    private float currentRelativeVelocity;
    public FloorPlayerBehaviour(PlayerStateMachine playerStateMachine, PlayerPhysics playerPhysics, PlayerController playerController) : base(playerStateMachine, playerPhysics, playerController) { }
    public override void Enter()
    {
        base.Enter();
        _playerController.ResetGroundFlags();
        currentRelativeVelocity = _playerPhysics.rb2D.velocity.x / _playerPhysics.playerPhysicsValues.moveMaxVelocity;
    }
    public override void Update()
    {
        _playerPhysics.Move(_playerController.m_playerInputHandler.absoluteMovementInput, currentRelativeVelocity);
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        
    }
}
