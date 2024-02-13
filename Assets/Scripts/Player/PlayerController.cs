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

    public GameObject bubbleInstance;
    public Transform instancedBubbleTransform;
    public BubbleController bubbleController;

    public GameObject currentBubble;
    public Transform currentBubbleTransform;

    public int facingDirection { get; private set; }

    public Vector3 respawnPosition { get; private set; }

    public float lastDashTime { get; private set; }

    public bool didPlayerTouchGroundSinceLastDash = true;

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
    public PlayerBubbledState bubbledState { get; private set; }
    #endregion

    #region Unity callback functions
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
        bubbledState = new PlayerBubbledState(this, stateMachine, m_playerValues, "bubbled");
    }


    void Start()
    {
        facingDirection = 1;
        respawnPosition = Vector3.zero;

        stateMachine.Initialize(idleState);

        bubbleController.Initialize(this, transform, m_playerValues);
        bubbleInstance.SetActive(false);
    }


    void Update()
    {
        stateMachine.currentState.Update();

        if (CheckIfCanBubble())
        {
            m_playerInputHandler.UseBubbleInput();
            InstantiateBubble();
        }
    }

    private void FixedUpdate()
    {
        stateMachine.currentState.FixedUpdate();
    }
    #endregion

    #region Player altering functions called by states

    /// <summary>
    /// All functions that the states can call on the PlayerController to alter it.
    /// For example, you can set the horizontal velocity to walk, or an initial vertical velocity to jump.
    /// </summary>

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }
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
        if (m_playerInputHandler.dashInput && m_playerValues.isDashUnlocked && Time.time > lastDashTime + m_playerValues.dashCooldownSeconds && didPlayerTouchGroundSinceLastDash)
        {
            m_playerInputHandler.UseDashInput();
            lastDashTime = Time.time;
            didPlayerTouchGroundSinceLastDash = false;
            return true;
        }
        return false;
    }

    public bool ResetDashGroundFlag() => didPlayerTouchGroundSinceLastDash = true;

    public bool CheckIfCanBubble()
    {
        if (m_playerInputHandler.bubbleInput && m_playerValues.isBubbleUnlocked && bubbleInstance.activeSelf == false)
        {
            m_playerInputHandler.UseBubbleInput();
            return true;
        }
        return false;
    }

    public bool CheckIfCanExpand()
    {
        if (m_playerInputHandler.expandInput && m_playerValues.isExpandUnlocked)
        {
            m_playerInputHandler.UseExpandInput();
            return true;
        }
        return false;
    }

    public void InstantiateBubble()
    {
        if (bubbleInstance.activeSelf == true)
        {
            bubbleInstance.SetActive(false);
        }

        bubbleInstance.SetActive(true);
        instancedBubbleTransform = bubbleInstance.transform;
        bubbleController = bubbleInstance.GetComponent<BubbleController>();
    }

    public void DestroyBubble()
    {
        bubbleController.popBubble();
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

    public bool checkIfGrounded()
    {
        return Physics2D.OverlapBox((Vector2)transform.position + m_playerValues.groundCheckOffset, m_playerValues.groundCheckBox, 0, m_playerValues.whatIsGround);
    }

    public bool checkIfTouchingFrontWall()
    {
        if (facingDirection == 1)
        {
            return checkIfTouchingRightWall();
        } else
        {
            return checkIfTouchingLeftWall();
        }
    }

    public bool checkIfTouchingRightWall()
    {
        return Physics2D.OverlapBox((Vector2)transform.position + m_playerValues.rightWallCheckOffset, m_playerValues.rightWallCheckBox, 0, m_playerValues.whatIsGround);
    }

    public bool checkIfTouchingLeftWall()
    {
        return Physics2D.OverlapBox((Vector2)transform.position + m_playerValues.leftWallCheckOffset, m_playerValues.leftWallCheckBox, 0, m_playerValues.whatIsGround);
    }

    public bool checkIfTouchingHazard()
    {
        return Physics2D.OverlapBox((Vector2)transform.position + m_playerValues.hazardCheckOffset, m_playerValues.hazardCheckBox, 0, m_playerValues.whatIsHazard);
    }

    public bool checkIfTouchingBubble()
    {
        return Physics2D.OverlapBox((Vector2)transform.position + m_playerValues.bubbleCheckOffset, m_playerValues.bubbleCheckBox, 0, m_playerValues.whatIsBubble);
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

        Gizmos.color = new Color(0, 0, 1, 0.4f);

        //Ground check and wall check area gizmos
        Gizmos.DrawCube((Vector2)transform.position + m_playerValues.groundCheckOffset, m_playerValues.groundCheckBox);

        Gizmos.DrawCube((Vector2)transform.position + m_playerValues.rightWallCheckOffset, m_playerValues.rightWallCheckBox);
        
        Gizmos.DrawCube((Vector2)transform.position + m_playerValues.leftWallCheckOffset, m_playerValues.leftWallCheckBox);

        //Hazard check area gizmo
        Gizmos.color = new Color(1, 0, 0, 0.4f);
        Gizmos.DrawCube((Vector2)transform.position + m_playerValues.hazardCheckOffset, m_playerValues.hazardCheckBox);

        //Bubble check area gizmo
        Gizmos.color = new Color(0, 1, 1, 0.3f);
        Gizmos.DrawCube((Vector2)transform.position + m_playerValues.bubbleCheckOffset, m_playerValues.bubbleCheckBox);
    }
    #endregion

}
