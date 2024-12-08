using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBehaviour 
{
    protected PlayerStateMachine _playerStateMachine;
    protected PlayerPhysics _playerPhysics;
    protected PlayerController _playerController;
    public float startingTime;
    
    public PlayerBehaviour (PlayerStateMachine playerStateMachine, PlayerPhysics playerPhysics,PlayerController playerController)
    {
        _playerStateMachine = playerStateMachine;
        _playerPhysics = playerPhysics;
        _playerController = playerController;
    }
    public virtual void Enter()
    {
        startingTime = Time.time;
        DoChecks();
    }
    public virtual void Logic()
    {

    }
    public virtual void Physics()
    {
        DoChecks();
        if (_playerPhysics.isGrounded)
        {
            _playerController.ResetGroundFlags();
        }
    }
    public virtual void DoChecks()
    {
        _playerPhysics.checkIfGrounded();
    }
    
}
