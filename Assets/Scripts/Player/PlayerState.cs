using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected PlayerController playerController;
    protected PlayerStateMachine playerStateMachine;
    protected PlayerValues playerValues;

    protected float startTime;

    private string animBoolName;

    protected bool isGrounded;

    public PlayerState(PlayerController playerController, PlayerStateMachine playerStateMachine, PlayerValues playerValues, string animBoolName)
    {
        this.playerController = playerController;
        this.playerStateMachine = playerStateMachine;
        this.playerValues = playerValues;
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
        isGrounded = playerController.checkIfGrounded();
    }
}
