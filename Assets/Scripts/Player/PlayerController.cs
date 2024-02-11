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

    public Vector3 respawnPosition { get; private set; }

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
    public PlayerDeathState deathState { get; private set; }
    public PlayerRespawnState respawnState { get; private set; }

    public PlayerDashState dashState { get; private set; }

    private void Awake()
    {
        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, m_playerValues, "idle");
        moveState = new PlayerMoveState(this, stateMachine, m_playerValues, "move");
        jumpState = new PlayerJumpState(this, stateMachine, m_playerValues, "jump");
        fallState = new PlayerFallState(this, stateMachine, m_playerValues, "fall");
        deathState = new PlayerDeathState(this, stateMachine, m_playerValues, "death");
        respawnState = new PlayerRespawnState(this, stateMachine, m_playerValues, "respawn");
        dashState = new PlayerDashState(this, stateMachine, m_playerValues, "dash");
    }

    #endregion


    void Start()
    {
        facingDirection = 1;
        respawnPosition = Vector3.zero;

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

    #region Player altering functions called by states

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

    public void SetGravityScale(float gravity)
    {
        m_rb2D.gravityScale = gravity;
    }

    public void SetLinearDrag(float linearDrag)
    {
        m_rb2D.drag = linearDrag;
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

    public bool CheckIfCanDash()
    {
        if (m_playerInputHandler.dashInput && m_playerValues.isDashUnlocked)
        {
            m_playerInputHandler.UseDashInput();
            return true;
        }
        return false;
    }

    public bool CheckIfCanBubble()
    {
        if (m_playerInputHandler.bubbleInput && m_playerValues.isBubbleUnlocked)
        {
            m_playerInputHandler.UseDashInput();
            return true;
        }
        return false;
    }

    public bool CheckIfCanExpand()
    {
        if (m_playerInputHandler.expandInput && m_playerValues.isExpandUnlocked)
        {
            m_playerInputHandler.UseDashInput();
            return true;
        }
        return false;
    }

    public void Respawn()
    {
        transform.position = respawnPosition;
    }

    public void FreezePlayerPosition(bool isPlayerFrozen)
    {
        if (isPlayerFrozen)
        {
            m_rb2D.constraints = RigidbodyConstraints2D.FreezeAll;
        } else
        {
            m_rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    #endregion

    #region Physics checks
    [Header("Check variables")]
    [SerializeField] Transform groundCheckTransform;
    [SerializeField] Transform hazardCheckTransform;


    public bool checkIfGrounded()
    {
        return Physics2D.OverlapBox(groundCheckTransform.position, m_playerValues.groundCheckBox, 0, m_playerValues.whatIsGround);
    }

    public bool checkIfTouchingHazard()
    {
        return Physics2D.OverlapBox(hazardCheckTransform.position, m_playerValues.hazardCheckBox, 0, m_playerValues.whatIsHazard);
    }

    #endregion

    #region Player altering functions called outside states

    public void SetRespawnPosition(Vector3 position)
    {
        respawnPosition = position;
    }

    #endregion

    #region Gizmos
    void OnDrawGizmos()
    {
        if (m_playerValues == null) return;

        //Ground check area gizmo
        Gizmos.color = new Color(0, 0, 1, 0.4f);
        if (groundCheckTransform != null)
        {
            Gizmos.DrawCube(groundCheckTransform.position, m_playerValues.groundCheckBox);
        }

        //Hazard check area gizmo
        Gizmos.color = new Color(1, 0, 0, 0.4f);
        if (hazardCheckTransform != null)
        {
            Gizmos.DrawCube(hazardCheckTransform.position, m_playerValues.hazardCheckBox);
        }
    }
    #endregion

}
