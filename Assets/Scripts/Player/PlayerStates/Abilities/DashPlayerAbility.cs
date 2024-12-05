using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashPlayerAbility : PlayerAbility
{
    public DashPlayerAbility(PlayerStateMachine playerStateMachine, PlayerPhysics playerPhysics, PlayerController playerController, AbilityPlayerBehaviour abilityPlayerBehaviour) : base(playerStateMachine, playerPhysics, playerController, abilityPlayerBehaviour)
    {
    }

    public override void Activate()
    {
        base.Activate();
        _playerPhysics.DashIn();
        _playerController.DashIn();
    }
    public override void Deactivate()
    {
        base.Deactivate();
        _playerPhysics.DashOut();
        _playerController.DashOut();
    }
    public override void Update()
    {
        base.Update();
        _playerPhysics.SetVelocityY(0);
        float elapsedTime = Time.time - startingTime;
        float dashDuration = _playerPhysics.playerPhysicsValues.dashTime;
        float fraction = elapsedTime / dashDuration;

        float currentDrag = Mathf.Lerp(_playerPhysics.playerPhysicsValues.dashLinearDrag, _playerPhysics.playerPhysicsValues.defaultLinearDrag, fraction);
        _playerPhysics.SetLinearDrag(currentDrag);

        if(elapsedTime > dashDuration)
        {
            _abilityPlayerBehaviour.AbilityOut();
        }
        
    }
}
