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
    }
    public override void Update()
    {
        base.Update();
        if(_playerController.CheckIfCanDash())
        {
            //Add bubble and expansion
            _playerStateMachine.ChangeState(_playerController.abilityState);
        }
        if (!_playerController.m_playerInputHandler.jumpInputHeld||_playerPhysics.rb2D.velocity.y<=0)
        {
            _playerStateMachine.ChangeState(_playerController.fallState);
        }
    }

}
