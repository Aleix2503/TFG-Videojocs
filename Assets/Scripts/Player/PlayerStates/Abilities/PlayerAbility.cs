using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerAbility : AbilityPlayerBehaviour
{
    public AbilityPlayerBehaviour _abilityPlayerBehaviour;

    protected PlayerAbility(PlayerStateMachine playerStateMachine, PlayerPhysics playerPhysics, PlayerController playerController,AbilityPlayerBehaviour abilityPlayerBehaviour) : base(playerStateMachine, playerPhysics, playerController)
    {
    }
    public virtual void Activate()
    {
        base.Enter();
    }
    public virtual void Deactivate()
    {
    }

}
