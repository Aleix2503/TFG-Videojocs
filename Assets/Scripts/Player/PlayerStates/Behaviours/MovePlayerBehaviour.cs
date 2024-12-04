using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayerBehaviour : PlayerBehaviour
{
    protected int horizontalInput;
    public MovePlayerBehaviour(PlayerStateMachine playerStateMachine, PlayerPhysics playerPhysics, PlayerController playerController) : base(playerStateMachine, playerPhysics, playerController) { }

    public override void Enter()
    {
        base.Enter();
    }
    public override void Update()
    {
        base.Update();
        horizontalInput = _playerController.m_playerInputHandler.absoluteMovementInput;
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        _playerController.Move(horizontalInput);
    }
}
