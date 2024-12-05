using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirPlayerBehaviour : PlayerBehaviour
{
    private float currentRelativeVelocity;
    public AirPlayerBehaviour(PlayerStateMachine playerStateMachine, PlayerPhysics playerPhysics, PlayerController playerController) : base(playerStateMachine, playerPhysics, playerController) { }
    public override void Enter()
    {
        base.Enter();
        currentRelativeVelocity = _playerPhysics.rb2D.velocity.x / _playerPhysics.playerPhysicsValues.airMoveMaxVelocity;
    }
    public override void Logic()
    {
        base.Logic();
        currentRelativeVelocity = _playerPhysics.AirMove(_playerController.m_playerInputHandler.absoluteMovementInput, currentRelativeVelocity);
    }
    public override void Physics()
    {
        base.Physics();
    }

}

    

