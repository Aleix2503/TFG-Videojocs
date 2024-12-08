using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashPlayerBehaviour : PlayerBehaviour
{
    public bool isTouchingFrontWall;
    public DashPlayerBehaviour(PlayerStateMachine playerStateMachine, PlayerPhysics playerPhysics, PlayerController playerController) : base(playerStateMachine, playerPhysics, playerController)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _playerPhysics.DashIn();
        _playerController.DashIn();
    }
    public void Exit()
    {
        _playerPhysics.DashOut();
        _playerController.DashOut();
        _playerStateMachine.ChangeState(_playerController.idleState);
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
            Exit();
        }
        if(isTouchingFrontWall)
        {
            PaintManager._instance.PlaceSplat(_playerPhysics.transform.position + new Vector3(0.5f * _playerPhysics.facingDirection, 0, 0),
                Vector3.left * _playerPhysics.facingDirection, _playerController.playerControlValues.dashColor);
            Exit();
        }

    }
}
