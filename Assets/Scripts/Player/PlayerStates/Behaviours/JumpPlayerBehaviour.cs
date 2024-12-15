using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPlayerBehaviour : AirPlayerBehaviour
{
    public JumpPlayerBehaviour(PlayerStateMachine playerStateMachine, PlayerPhysics playerPhysics, PlayerController playerController) : base(playerStateMachine, playerPhysics, playerController)
    {
    }
    public override void Enter()
    {
        base.Enter();
        _playerPhysics.Jump();
        PaintManager._instance.EmitJumpParticles();
    }
    public override void Logic()
    {
        base.Logic();
        if (!_playerController.m_playerInputHandler.jumpInputHeld || _playerPhysics.rb2D.velocity.y <= 0)
        {
            _playerStateMachine.ChangeState(_playerController.fallState);
            return;
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
        if (_playerController.CheckIfCanCube())
        {
            _playerStateMachine.ChangeState(_playerController.cubeState);
            return;
        }
        
    }

}
