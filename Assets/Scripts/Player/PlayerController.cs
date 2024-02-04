using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D m_rb2D;
    public PlayerInputHandler m_playerInputHandler;

    public PlayerStateMachine playerStateMachine { get; private set; }
    
    public Animator animator;

    private void Awake()
    {
        playerStateMachine = new PlayerStateMachine();
    }

    

    void Start()
    {
        //Initialize State machine
    }


    void Update()
    {
        
        print(m_playerInputHandler.movementInput);

        playerStateMachine.currentState.Update();
    }

    private void FixedUpdate()
    {
        playerStateMachine.currentState.FixedUpdate();
    }
}
