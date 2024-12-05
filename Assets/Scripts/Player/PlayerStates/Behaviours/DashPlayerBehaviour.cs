using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashPlayerBehaviour : PlayerBehaviour
{
    public DashPlayerBehaviour(PlayerStateMachine playerStateMachine, PlayerPhysics playerPhysics, PlayerController playerController) : base(playerStateMachine, playerPhysics, playerController)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _playerPhysics.DashIn();
        _playerController.DashIn();
    }
    public override void Logic()
    {
        base.Logic();
        _playerPhysics.SetVelocityY(0);
        float elapsedTime = Time.time - startingTime;
        float dashDuration = _playerPhysics.playerPhysicsValues.dashTime;
        float fraction = elapsedTime / dashDuration;

        float currentDrag = Mathf.Lerp(_playerPhysics.playerPhysicsValues.dashLinearDrag, _playerPhysics.playerPhysicsValues.defaultLinearDrag, fraction);

        _playerPhysics.SetLinearDrag(currentDrag);

        if(elapsedTime > dashDuration)
        {
            _playerPhysics.DashOut();
            _playerController.DashOut();
            _playerStateMachine.ChangeState(_playerController.idleState);
        }
        
    }
}
