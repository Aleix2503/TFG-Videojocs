using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BubblePlayerBehaviour : PlayerBehaviour
{
    private float currentRelativeVelocity;

    private bool isTouchingWall;
    private bool isTouchingCeiling;

    private int bounceCounter;

    private bool isBubblingOut;
    private float bubbleOutTimer;

    private float bounceTimer;
    private bool hasExploded;
    public BubblePlayerBehaviour(PlayerStateMachine playerStateMachine, PlayerPhysics playerPhysics, PlayerController playerController) : base(playerStateMachine, playerPhysics, playerController)
    {

    }

    public override void Enter()
    {
        base.Enter();
        _playerPhysics.BubbleIn();
        _playerController.BubbleIn();
        currentRelativeVelocity = _playerPhysics.rb2D.velocity.x / _playerPhysics.playerPhysicsValues.bubbleHorizontalVelocity;
        isBubblingOut = false;
        bounceCounter = 0;
        hasExploded = false;
        isBouncingH = false;
        isBouncingV = false;
        bounceTimer = 0;
        isTouchingCeiling = false;
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
        _playerStateMachine.SetAnimBool("isBubbling",false);
        _playerStateMachine.SetSound("bubble");

    }
    private bool itSounded= false;
    private void PlayBounceSound()
    {
        if (itSounded)
        {
            _playerStateMachine.SetSound("bounceBubble");
            itSounded = false;
        }
    }

    private PlayerBehaviour _nextBehaviour;
    private bool isBouncingH;
    private bool isBouncingV;
    private float Ydestiny;
    private float Xdestiny;
    public override void Logic()
    {
        base.Logic();
        if((!_playerController.m_playerInputHandler.bubbleInputHeld ||hasExploded)&&!isBubblingOut)
        {
            PreparePop();
            if (!hasExploded) { _nextBehaviour = _playerController.idleState;}
            PaintManager._instance.EmitBubbleParticles();
        }
        bubbleOutTimer -= Time.deltaTime;
        bounceTimer -= Time.deltaTime;
        if (isBubblingOut && bubbleOutTimer<=0)
        {
            _playerPhysics.FreezePlayerPosition(false);
            _playerPhysics.SetGravityScale(_playerPhysics.playerPhysicsValues.defaultGravity);
            _playerStateMachine.ChangeState(_nextBehaviour);
            return;
        }

        //Fall Behaviour Backwards
        if (_playerPhysics.rb2D.velocity.y < 0&&!isTouchingCeiling) { _playerPhysics.ImpulseBubble(); }
        else if(!isTouchingCeiling) { _playerPhysics.Float(); }
        if(_playerPhysics.rb2D.velocity.y <= _playerPhysics.playerPhysicsValues.floatTerminalVelocity&&!isTouchingCeiling)
        {
            _playerPhysics.BubbleMaxSpeed();
        }

        //Move Behaviour
        currentRelativeVelocity = _playerPhysics.BubbleMove(_playerController.m_playerInputHandler.absoluteMovementInput, currentRelativeVelocity);

        if(isBouncingH)
        {
            isBouncingH = _playerPhysics.BounceHorizontal(Xdestiny);
            if (_playerPhysics.facingDirection == 1)
            {
                if (_playerPhysics.checkIfTouchingLeftWall())
                {
                    isBouncingH = false;
                }
            }
            else
            {
                if (_playerPhysics.checkIfTouchingRightWall())
                {
                    isBouncingH = false;
                }
            }
        }
        if (isBouncingV)
        {
            isBouncingV = _playerPhysics.BounceVertical(Ydestiny);
            if (_playerPhysics.isGrounded)
            {
                isBouncingV = false;
            }
        }
        if (bounceCounter<= _playerController.playerControlValues.bubbleMaxBounces&&isTouchingCeiling&&bounceTimer<=0&&!isBouncingV)
        {
            bounceCounter++;
            bounceTimer = 0.05f;
            PaintManager._instance.PlaceSplat(_playerPhysics.transform.position + new Vector3(0, 0.3f, 0), Vector3.down, _playerController.playerControlValues.bubbleColor);

            Ydestiny = _playerPhysics.rb2D.position.y - (_playerPhysics.playerPhysicsValues.bubbleVerticalBounceForce * (_playerPhysics.playerPhysicsValues.bubbleBounceTime / 2));
            isBouncingV = _playerPhysics.BounceVertical(Ydestiny);
            itSounded = true;
            PlayBounceSound();
            PaintManager._instance.EmitBubbleParticles();
            _playerStateMachine.SetAnimTrigger("isBouncing");
        }
        if(bounceCounter <= _playerController.playerControlValues.bubbleMaxBounces&&isTouchingWall&&bounceTimer <= 0&& !isBouncingH)
        {
            bounceCounter++;
            bounceTimer = 0.05f;
            PaintManager._instance.PlaceSplat(_playerController.transform.position + new Vector3(0.5f * _playerPhysics.facingDirection, 0, 0),
                Vector3.left * _playerPhysics.facingDirection, _playerController.playerControlValues.bubbleColor);

            Xdestiny = _playerPhysics.rb2D.position.x - (_playerPhysics.playerPhysicsValues.bubbleHorizontalBounceForce * (_playerPhysics.playerPhysicsValues.bubbleBounceTime / 2) * _playerPhysics.facingDirection);
            isBouncingH = _playerPhysics.BounceHorizontal(Xdestiny);
            itSounded = true;
            PlayBounceSound();
            PaintManager._instance.EmitBubbleParticles();
            _playerStateMachine.SetAnimTrigger("isBouncing");
        }
        else if(bounceCounter > _playerController.playerControlValues.bubbleMaxBounces)
        {
            hasExploded = true;
            _nextBehaviour = _playerController.idleState;
        }
        if (_playerController.CheckIfCanDash())
        {
            hasExploded = true;
            _nextBehaviour = _playerController.dashState;
        }
        if(_playerController.CheckIfCanCube())
        {
            hasExploded = true;
            _nextBehaviour = _playerController.cubeState;
        }
    }
    public override void Physics()
    {
        base.Physics();
    }
    public override void DoChecks()
    {
        base.DoChecks();
        isTouchingWall = _playerPhysics.checkIfTouchingFrontWall();
        isTouchingCeiling = _playerPhysics.checkIfBubbleTouchingCeiling();
    }

}
