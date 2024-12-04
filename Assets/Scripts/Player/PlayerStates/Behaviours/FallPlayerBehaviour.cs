using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallPlayerBehaviour : MovePlayerBehaviour
{
    public FallPlayerBehaviour(PlayerStateMachine playerStateMachine, PlayerPhysics playerPhysics, PlayerController playerController) : base(playerStateMachine, playerPhysics, playerController)
    {
    }
}
