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
    protected bool isTouchingHazard;

    public PlayerState(PlayerController playerController, PlayerStateMachine playerStateMachine, PlayerValues playerValues, string animBoolName)
    {
        this.playerController = playerController;
        this.playerStateMachine = playerStateMachine;
        this.playerValues = playerValues;
        this.animBoolName = animBoolName;
    }

    //Called from PlayerStateMachine. 
    public virtual void Enter()
    {
        DoChecks();
        startTime = Time.time;

        playerController.animator.SetBool(animBoolName, true);
    }

    //Called from PlayerStateMachine. 
    public virtual void Exit()
    {
        playerController.animator.SetBool(animBoolName, false);
    }

    //Called from PlayerController. 
    public virtual void Update()
    {

    }

    //Called from PlayerController. 
    public virtual void FixedUpdate()
    {
        DoChecks();

        if (isTouchingHazard)
        {
            playerStateMachine.ChangeState(playerController.deathState);
        }
    }

    //Called from this class, in Enter and FixedUpdate. Reserve for calling specific physics checks on the player class.
    public virtual void DoChecks()
    {
        isGrounded = playerController.checkIfGrounded();
        isTouchingHazard = playerController.checkIfTouchingHazard();
    }
}
