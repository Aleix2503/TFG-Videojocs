using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPlayerBehaviour : MovePlayerBehaviour
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
        
    }

}
