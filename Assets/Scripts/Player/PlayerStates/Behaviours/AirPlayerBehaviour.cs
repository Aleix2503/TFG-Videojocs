using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirPlayerBehaviour : PlayerBehaviour
{
    private float currentRelativeVelocity;
    private bool isTouchingFrontWall = false;

    private float lastWallPaintTime = 0;
    public AirPlayerBehaviour(PlayerStateMachine playerStateMachine, PlayerPhysics playerPhysics, PlayerController playerController) : base(playerStateMachine, playerPhysics, playerController) { }
    public override void Enter()
    {
        base.Enter();
        currentRelativeVelocity = _playerPhysics.rb2D.velocity.x / _playerPhysics.playerPhysicsValues.airMoveMaxVelocity;
    }
    public override void Logic()
    {
        base.Logic();
        if (isTouchingFrontWall)
        {
            TryPaintFrontWall();
        }
        currentRelativeVelocity = _playerPhysics.AirMove(_playerController.m_playerInputHandler.absoluteMovementInput, currentRelativeVelocity);
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
    private void TryPaintFrontWall()
    {
        if (Time.time > lastWallPaintTime + 0.2f)
        {
            PaintManager._instance.PlaceSplat(_playerController.transform.position + new Vector3(0.5f * _playerPhysics.facingDirection, 0, 0),
                Vector3.left * _playerPhysics.facingDirection, _playerController.playerControlValues.defaultColor);

            lastWallPaintTime = Time.time;
        }
    }
}

    

