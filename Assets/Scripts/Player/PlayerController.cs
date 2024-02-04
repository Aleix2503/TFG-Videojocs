using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerValues m_playerValues;

    public Rigidbody2D m_rb2D;
    public PlayerInputHandler m_playerInputHandler;

    public PlayerStateMachine stateMachine { get; private set; }
    
    public Animator animator;

    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }

    private void Awake()
    {
        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, m_playerValues, "idle");
        moveState = new PlayerMoveState(this, stateMachine, m_playerValues, "move");
    }


    void Start()
    {
        stateMachine.Initialize(idleState);
    }


    void Update()
    {
        stateMachine.currentState.Update();
    }

    private void FixedUpdate()
    {
        stateMachine.currentState.FixedUpdate();
    }
}
