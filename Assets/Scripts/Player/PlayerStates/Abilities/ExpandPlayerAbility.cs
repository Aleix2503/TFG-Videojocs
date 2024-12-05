using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandPlayerAbility : PlayerAbility
{
    public ExpandPlayerAbility(PlayerStateMachine playerStateMachine, PlayerPhysics playerPhysics, PlayerController playerController, AbilityPlayerBehaviour abilityPlayerBehaviour) : base(playerStateMachine, playerPhysics, playerController, abilityPlayerBehaviour)
    {
    }

    public override void Activate()
    {
        base.Activate();
    }
    public override void Update()
    {
        base.Update();
    }

}
