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

    public int facingDirection { get; private set; }


    #region State machine setup

    /// <summary>
    /// PlayerController contains and initializes instances of all states.
    /// Whenever a state change happens, it gets replaced by one of these.
    /// The name of the animation parameter is set when instantiating the state in the "Awake" method.
    /// </summary>
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerFallState fallState { get; private set; }

    private void Awake()
    {
        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, m_playerValues, "idle");
        moveState = new PlayerMoveState(this, stateMachine, m_playerValues, "move");
        jumpState = new PlayerJumpState(this, stateMachine, m_playerValues, "jump");
        fallState = new PlayerFallState(this, stateMachine, m_playerValues, "fall");
    }

    #endregion


    void Start()
    {
        facingDirection = 1;

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

    #region Player altering functions

    /// <summary>
    /// All functions that the states can call on the PlayerController to alter it.
    /// For example, you can set the horizontal velocity to walk, or an initial vertical velocity to jump.
    /// </summary>

    public void SetVelocityX(float velocity)
    {
        Vector2 newVelocity = new Vector2(velocity, m_rb2D.velocity.y);
        m_rb2D.velocity = newVelocity;
    }

    public void SetVelocityY(float velocity)
    {
        Vector2 newVelocity = new Vector2(m_rb2D.velocity.x, velocity);
        m_rb2D.velocity = newVelocity;
    }

    public void CheckIfShouldFlip(float movementInput)
    {
        if (movementInput == 0) return;

        int direction = movementInput > 0 ? 1 : -1;

        if (direction != facingDirection)
        {
            Flip();
        }
    }

    private void Flip()
    {
        facingDirection *= -1;
        transform.Rotate(0, 180, 0);
    }

    #endregion

    #region Physics checks
    [Header("Check variables")]
    [SerializeField]
    Transform groundCheckTransform;


    public bool checkIfGrounded()
    {
        return Physics2D.OverlapBox(groundCheckTransform.position, m_playerValues.groundCheckBox, 0, m_playerValues.whatIsGround);
    }

    #endregion
}
