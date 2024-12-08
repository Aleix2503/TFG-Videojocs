using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubblePlayerBehaviour : PlayerBehaviour
{
    private float currentRelativeVelocity;
    private bool isTouchingWall;
    private bool isTouchingCeiling;
    private int bounceCounter;
    public BubblePlayerBehaviour(PlayerStateMachine playerStateMachine, PlayerPhysics playerPhysics, PlayerController playerController) : base(playerStateMachine, playerPhysics, playerController)
    {

    }

    public override void Enter()
    {
        base.Enter();
        _playerStateMachine.SetAnim(2);
        _playerPhysics.BubbleIn();
        _playerController.BubbleIn();
        currentRelativeVelocity = _playerPhysics.rb2D.velocity.x / _playerPhysics.playerPhysicsValues.bubbleHorizontalVelocity;
        isBubblingOut = false;
        bounceCounter = 0;
        hasExploded = false;
    }
    public void Exit()
    {
        _playerPhysics.BubbleOut();
        _playerController.BubbleOut();
    }
    private void PreparePop()
    {
        Exit();
        bubbleOutTimer = _playerPhysics.playerPhysicsValues.bubbleTransformationTime;
        isBubblingOut = true;
        _playerStateMachine.SetAnim(3);
    }
    private bool isBubblingOut;
    private float bubbleOutTimer;
    private float bounceTimer;
    private bool hasExploded;
    public override void Logic()
    {
        base.Logic();
        if((!_playerController.m_playerInputHandler.bubbleInputHeld ||hasExploded)&&!isBubblingOut)
        {
            PreparePop();
        }
        bubbleOutTimer -= Time.deltaTime;
        bounceTimer -= Time.deltaTime;
        if (isBubblingOut && bubbleOutTimer<=0)
        {
            _playerPhysics.FreezePlayerPosition(false);
            _playerPhysics.SetGravityScale(_playerPhysics.playerPhysicsValues.defaultGravity);
            _playerStateMachine.ChangeState(_playerController.idleState);
            return;
        }
        if (_playerPhysics.rb2D.velocity.y < 0&&!isTouchingCeiling) { _playerPhysics.ImpulseBubble(); }
        else if(!isTouchingCeiling) { _playerPhysics.Float(); }
        if(_playerPhysics.rb2D.velocity.y <= _playerPhysics.playerPhysicsValues.floatTerminalVelocity&&!isTouchingCeiling)
        {
            _playerPhysics.BubbleMaxSpeed();
        }
        if(!isTouchingWall)currentRelativeVelocity = _playerPhysics.BubbleMove(_playerController.m_playerInputHandler.absoluteMovementInput, currentRelativeVelocity);
        if(bounceCounter<= _playerController.playerControlValues.bubbleMaxBounces&&(isTouchingCeiling||isTouchingWall)&&bounceTimer<=0)
        {
            bounceCounter++;
            bounceTimer = 0.5f;
            if (isTouchingWall)
            {
                _playerPhysics.BounceHorizontal();
            }
            if (isTouchingCeiling)
            {
                _playerPhysics.BounceVertical();
            }
        }
        else if(bounceCounter > _playerController.playerControlValues.bubbleMaxBounces)
        {
            hasExploded = true;
        }
    }
    public override void DoChecks()
    {
        base.DoChecks();
        isTouchingWall = _playerPhysics.checkIfTouchingFrontWall();
        isTouchingCeiling = _playerPhysics.checkIfTouchingCeiling();
    }

}
