using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlayerBehaviour : PlayerBehaviour
{
    public bool isDying;
    public DeathPlayerBehaviour(PlayerStateMachine playerStateMachine, PlayerPhysics playerPhysics, PlayerController playerController) : base(playerStateMachine, playerPhysics, playerController) { }
    public override void Enter()
    {
        base.Enter();
        _playerPhysics.SetVelocityX(0);
        _playerPhysics.SetVelocityY(0);
        PaintManager._instance.PlaceSplat(_playerController.transform.position + new Vector3(0, -0.5f, 0),
               Vector3.up, _playerController.currentPlayerColor);
        PaintManager._instance.PlaceSplat(_playerController.transform.position + new Vector3(0.5f, 0, 0),
                Vector3.left, _playerController.currentPlayerColor);
        PaintManager._instance.PlaceSplat(_playerController.transform.position + new Vector3(0, 0.5f, 0),
                Vector3.down, _playerController.currentPlayerColor);
        PaintManager._instance.PlaceSplat(_playerController.transform.position + new Vector3(-0.5f, 0, 0),
            Vector3.right, _playerController.currentPlayerColor);
        _playerPhysics.FreezePlayerPosition(true);
        isDying = true;
    }
    public void Exit()
    {
        _playerController.FadePlayerColor(_playerController.playerControlValues.defaultColor, 0);
        _playerController.Respawn();
        _playerStateMachine.SetAnimTrigger("isRespawning");
    }
    public override void Logic()
    {
        base.Logic();
        if (Time.time> startingTime + _playerController.playerControlValues.deathToRespawnSeconds&&isDying)
        {
            Exit();
            isDying = false;
        }
        if (Time.time > startingTime + _playerController.playerControlValues.respawnToIdleSeconds+ _playerController.playerControlValues.deathToRespawnSeconds)
        {
            _playerStateMachine.ChangeState(_playerController.idleState);
            _playerPhysics.FreezePlayerPosition(false);
            _playerPhysics.SetGravityScale(_playerPhysics.playerPhysicsValues.defaultGravity);
            _playerPhysics.SetLinearDrag(_playerPhysics.playerPhysicsValues.defaultLinearDrag);
        }
    }
    public override void Physics()
    {
        
    }

}
