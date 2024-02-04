using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected PlayerController playerController;
    protected PlayerStateMachine playerStateMachine;

    protected float startTime;

    private string animBoolName;

    public PlayerState(PlayerController playerController, PlayerStateMachine playerStateMachine, string animBoolName)
    {
        this.playerController = playerController;
        this.playerStateMachine = playerStateMachine;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        DoChecks();
        startTime = Time.time;

        playerController.animator.SetBool(animBoolName, true);
    }

    public virtual void Exit()
    {
        playerController.animator.SetBool(animBoolName, false);
    }

    public virtual void Update()
    {

    }

    public virtual void FixedUpdate()
    {
        DoChecks();
    }

    public virtual void DoChecks()
    {

    }
}
