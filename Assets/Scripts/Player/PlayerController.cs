using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerValues m_playerValues;

    public Rigidbody2D m_rb2D;
    public BoxCollider2D m_collider2D;
    public SpriteRenderer m_spriteRenderer;
    public PlayerInputHandler m_playerInputHandler;
    public PlayerStateMachine stateMachine { get; private set; }
    
    public Animator animator;

    public GameObject bubbleInstance;
    public Transform instancedBubbleTransform;
    public BubbleController bubbleController;

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

    public PlayerSummonBubbleState summonBubbleState { get; private set; }
    public PlayerBubbledState bubbledState { get; private set; }

    public PlayerStartExpandState startExpandState { get; private set; }
    public PlayerExpandedState expandedState { get; private set; }
    public PlayerEndExpandState endExpandState { get; private set; }

    public PlayerNoMoveFallState noMoveFallState { get; private set; }
    public PlayerNoMoveIdleState noMoveIdleState { get; private set; }
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

        summonBubbleState = new PlayerSummonBubbleState(this, stateMachine, m_playerValues, "summonBubble");
        bubbledState = new PlayerBubbledState(this, stateMachine, m_playerValues, "bubbled");

        startExpandState = new PlayerStartExpandState(this, stateMachine, m_playerValues, "startExpand");
        expandedState = new PlayerExpandedState(this, stateMachine, m_playerValues, "expanded");
        endExpandState = new PlayerEndExpandState(this, stateMachine, m_playerValues, "endExpand");

        noMoveIdleState = new PlayerNoMoveIdleState(this, stateMachine, m_playerValues, "idle");
        noMoveFallState = new PlayerNoMoveFallState(this, stateMachine, m_playerValues, "fall");
    }


    void Start()
    {
        facingDirection = 1;
        respawnPosition = Vector3.zero;

        SetColliderDimensions(m_playerValues.defaultCollisionBox, m_playerValues.defaultCollisionBoxOffset, m_playerValues.defaultCollisionEdgeRadius);
        SetGravityScale(m_playerValues.defaultGravity);

        stateMachine.Initialize(idleState);
        SetPlayerNoMoveForSeconds(5);

        bubbleController.Initialize(this, transform, m_playerValues);
    }


    void Update()
    {
        stateMachine.currentState.Update();
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
        if (m_playerInputHandler.bubbleInput && m_playerValues.isBubbleUnlocked && bubbleController.isActive == false)
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

    public void SetColliderDimensions(Vector2 size, Vector2 offset, float edgeRadius)
    {
        m_collider2D.size = size;
        m_collider2D.offset = offset;
        m_collider2D.edgeRadius = edgeRadius;
    }
 
    public void InstantiateBubble()
    {
        bubbleController.SummonBubble();
        instancedBubbleTransform = bubbleInstance.transform;
        bubbleController = bubbleInstance.GetComponent<BubbleController>();
    }

    public void DestroyBubble()
    {
        bubbleController.PopBubble();
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

    private Coroutine fadePlayerCoroutine;
    public void FadePlayerColor(Color toColor, float seconds)
    {
        if (fadePlayerCoroutine != null)
        {
            StopCoroutine(fadePlayerCoroutine);
        }

        fadePlayerCoroutine = StartCoroutine(FadeToColorCoroutine(toColor, seconds));
    }

    private IEnumerator FadeToColorCoroutine(Color toColor, float seconds)
    {
        float time = 0;
        Color startColor = m_spriteRenderer.color;

        while (time < seconds)
        {
            m_spriteRenderer.color = Color.Lerp(startColor, toColor, time / seconds);
            time += Time.deltaTime;
            yield return null;
        }

        m_spriteRenderer.color = toColor;
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

    public bool checkIfExpandedCollision()
    {
        return Physics2D.OverlapBox((Vector2)transform.position + m_playerValues.expandedCollisionBoxOffset, m_playerValues.expandedCollisionBox, 0, m_playerValues.whatIsGround);
    }

    public bool checkIfExpandedTouchingHazard()
    {
        return Physics2D.OverlapBox((Vector2)transform.position + m_playerValues.expandedHazardCollisionBoxOffset, m_playerValues.expandedHazardCollisionBox, 0, m_playerValues.whatIsHazard);
    }

    public bool checkIfExpandedTouchingGround()
    {
        return Physics2D.OverlapBox((Vector2)transform.position + m_playerValues.expandedGroundCheckBoxOffset, m_playerValues.expandedGroundCheckBox, 0, m_playerValues.whatIsGround);
    }

    #endregion

    #region Player altering functions called outside states

    public void SetRespawnPosition(Vector3 position)
    {
        respawnPosition = position;
    }

    public void SetPlayerNoMoveForSeconds(float seconds)
    {
        if (checkIfGrounded())
        {
            noMoveIdleState.secondsLeft = seconds;
            stateMachine.ChangeState(noMoveIdleState);
        } else
        {
            noMoveFallState.secondsLeft = seconds;
            stateMachine.ChangeState(noMoveFallState);
        }
    }

    #endregion

    #region Gizmos
    void OnDrawGizmos()
    {
        if (m_playerValues == null) return;
        if (!m_playerValues.showGizmos) return;
        
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

        //Collision gizmo
        Gizmos.color = new Color(0, 1, 0, 0.4f);
        Gizmos.DrawCube((Vector2)transform.position + m_playerValues.defaultCollisionBoxOffset,
            new Vector2(m_playerValues.defaultCollisionBox.x + m_playerValues.defaultCollisionEdgeRadius, m_playerValues.defaultCollisionBox.y + m_playerValues.defaultCollisionEdgeRadius));

        if (m_playerValues.expandedShowGizmos)
        {
            //Expanded collision gizmos
            Gizmos.color = new Color(0.5f, 1, 0, 0.2f);
            Gizmos.DrawWireCube((Vector2)transform.position + m_playerValues.expandedCollisionBoxOffset,
                new Vector2(m_playerValues.expandedCollisionBox.x + m_playerValues.expandedCollisionEdgeRadius, m_playerValues.expandedCollisionBox.y + m_playerValues.expandedCollisionEdgeRadius));

            Gizmos.color = new Color(1, 0, 0, 0.2f);
            Gizmos.DrawWireCube((Vector2)transform.position + m_playerValues.expandedHazardCollisionBoxOffset, m_playerValues.expandedHazardCollisionBox);

            Gizmos.color = new Color(0, 0, 1, 0.2f);
            Gizmos.DrawWireCube((Vector2)transform.position + m_playerValues.expandedGroundCheckBoxOffset, m_playerValues.expandedGroundCheckBox);
        }
    }
    #endregion

}
