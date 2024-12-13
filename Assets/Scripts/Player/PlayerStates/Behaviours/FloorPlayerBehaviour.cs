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
        currentRelativeVelocity = _playerPhysics.rb2D.velocity.x / _playerPhysics.playerPhysicsValues.moveMaxVelocity;
    }
    public override void Logic()
    {
        base.Logic();
        currentRelativeVelocity = _playerPhysics.FloorMove(_playerController.m_playerInputHandler.absoluteMovementInput, currentRelativeVelocity);
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
