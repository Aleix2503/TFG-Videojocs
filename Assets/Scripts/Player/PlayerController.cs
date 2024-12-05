using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerPhysics playerPhysics;
    public PlayerControlValues playerControlValues;
    public SpriteRenderer m_spriteRenderer;
    public PlayerInputHandler m_playerInputHandler;
    public PlayerStateMachine stateMachine;

    /*public GameObject bubbleInstance;
    public Transform instancedBubbleTransform;
    public BubbleController bubbleController;*/

    public Vector3 respawnPosition { get; private set; }

    public float lastDashTime { get; private set; }

    public bool isCoyoteTimeActive = false;

    public bool didPlayerTouchGroundSinceLastDash = true;
    public bool didPlayerTouchGroundSinceLastExpand = true;

    public bool isPlayerLocked = false;

    public bool isDashUnlocked = false;
    public bool isBubbleUnlocked = false;
    public bool isExpandUnlocked = false;


    public Color currentPlayerColor => m_spriteRenderer.color;

    #region State machine setup

    /// <summary>
    /// PlayerController contains and initializes instances of all states.
    /// Whenever a state change happens, it gets replaced by one of these.
    /// The name of the animation parameter is set when instantiating the state in the "Awake" method.
    /// </summary>
    public IdlePlayerBehaviour idleState { get; private set; }
    public MovePlayerBehaviour moveState { get; private set; }
    public JumpPlayerBehaviour jumpState { get; private set; }
    public FallPlayerBehaviour fallState { get; private set; }
    public AbilityPlayerBehaviour abilityState { get; private set; }

    #endregion

    #region Unity callback functions

    public void Start()
    {
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
        playerPhysics = GetComponent<PlayerPhysics>();
        stateMachine = new();

        idleState = new IdlePlayerBehaviour(stateMachine, playerPhysics, this);
        moveState = new MovePlayerBehaviour(stateMachine, playerPhysics, this);
        jumpState = new JumpPlayerBehaviour(stateMachine, playerPhysics, this);
        fallState = new FallPlayerBehaviour(stateMachine, playerPhysics, this);
        abilityState = new AbilityPlayerBehaviour(stateMachine, playerPhysics, this);
        
        respawnPosition = Vector3.zero;

        stateMachine.Initialize(this, GetComponentInChildren<Animator>());
        SetPlayerNoMoveForSeconds(playerControlValues.initialNoControlTime);

        m_playerInputHandler.Initialize(playerControlValues);

        //bubbleController.Initialize(this, transform, playerPhysics.playerValues);

        if (playerControlValues.unlockAllAbilities)
        {
            isDashUnlocked = true;
            isBubbleUnlocked = true;
            isExpandUnlocked = true;
        }
    }


    void Update()
    {
        stateMachine.currentBehaviour.Update();
    }

    private void FixedUpdate()
    {
        stateMachine.currentBehaviour.FixedUpdate();
    }
    #endregion

    #region Player altering functions called by states

    /// <summary>
    /// All functions that the states can call on the PlayerController to alter it.
    /// For example, you can set the horizontal velocity to walk, or an initial vertical velocity to jump.
    /// </summary>
    public bool canDash = false;
    public bool CheckIfCanDash()
    {
        if (m_playerInputHandler.dashInput && isDashUnlocked && Time.time > lastDashTime + playerControlValues.dashCooldownSeconds && didPlayerTouchGroundSinceLastDash)
        {
            lastDashTime = Time.time;
            didPlayerTouchGroundSinceLastDash = false;
            canDash = true;
            return true;
        }
        canDash = false;
        return false;
    }
    public void ResetGroundFlags()
    {
        didPlayerTouchGroundSinceLastDash = true;
        didPlayerTouchGroundSinceLastExpand = true;
    }

    public bool canBubble = false;
    public bool CheckIfCanBubble()
    {
        /*if (m_playerInputHandler.bubbleInput && isBubbleUnlocked && bubbleController.isActive == false)
        {
            canBubble = true;
            return true;
        }*/
        canBubble = false;
        return false;
    }
    public bool canExpand = false;
    public bool CheckIfCanExpand()
    {
        if (m_playerInputHandler.expandInput && isExpandUnlocked && didPlayerTouchGroundSinceLastExpand)
        {
            didPlayerTouchGroundSinceLastExpand = false;
            canExpand = true;
            return true;
        }
        canExpand = false;
        return false;
    }
    public void DashIn()
    {
        FadePlayerColor(playerControlValues.dashColor, playerControlValues.dashColorFadeInTime);
    }
    public void DashOut()
    {
        FadePlayerColor(playerControlValues.defaultColor, playerControlValues.dashColorFadeOutTime);
    }

    /*public void InstantiateBubble()
    {
        bubbleController.SummonBubble();
        instancedBubbleTransform = bubbleInstance.transform;
        bubbleController = bubbleInstance.GetComponent<BubbleController>();
    }

    public void DestroyBubble()
    {
        bubbleController.PopBubble();
    }*/

    public void Respawn()
    {
        transform.position = respawnPosition;
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

    public void SetPaintingState(PlayerPaintingState paintingState)
    {
        PaintManager._instance.SetPaintingState(paintingState);
    }

    #endregion

    #region Player altering functions called outside states

    public void CheckCoyoteTime(float startTime)
    {
        if (isCoyoteTimeActive && Time.time > startTime + playerControlValues.coyoteTime)
        {
            isCoyoteTimeActive = false;
        }
    }
    public void StartCoyoteTime() => isCoyoteTimeActive = true;
    public void SetRespawnPosition(Vector3 position)
    {
        respawnPosition = position;
    }

    public void SetPlayerNoMoveForSeconds(float seconds)
    {
        if (playerPhysics.isGrounded)
        {
            //noMoveIdleState.secondsLeft = seconds;
            //stateMachine.ChangeState(noMoveIdleState);
        } else
        {
            //noMoveFallState.secondsLeft = seconds;
            //stateMachine.ChangeState(noMoveFallState);
        }
    }

    public void DisablePlayerControls()
    {
        m_playerInputHandler.DisablePlayerInput();
    }

    public void EnablePlayerControls()
    {
        m_playerInputHandler.EnablePlayerInput();
    }

    public void UnlockAbility(AbilityType ability)
    {
        SetPlayerNoMoveForSeconds(playerControlValues.abilityUnlockNoControlTime);

        switch (ability)
        {
            case AbilityType.Dash:
                isDashUnlocked = true;
                break;
            case AbilityType.Bubble:
                isBubbleUnlocked = true;
                break;
            case AbilityType.Expand:
                isExpandUnlocked = true;
                break;
            default:
                print("Non-existing ability?: " + ability.ToString());
                break;
        }
    }

    public enum AbilityType
    {
        Dash,
        Bubble,
        Expand
    }

    #endregion

    #region Gizmos
    /*void OnDrawGizmos()
    {
        if (m_playerValues == null) return;
        if (!m_playerValues.showGizmos) return;
        
        Gizmos.color = new Color(0, 0, 1, 0.4f);

        //Ground check and wall check area gizmos
        Gizmos.DrawCube((Vector2)transform.position + m_playerValues.groundCheckOffset, m_playerValues.groundCheckBox);

        Gizmos.DrawCube((Vector2)transform.position + m_playerValues.rightWallCheckOffset, m_playerValues.rightWallCheckBox);
        
        Gizmos.DrawCube((Vector2)transform.position + m_playerValues.leftWallCheckOffset, m_playerValues.leftWallCheckBox);

        Gizmos.DrawCube((Vector2)transform.position + m_playerValues.ceilingCheckOffset, m_playerValues.ceilingCheckBox);

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
    }*/
    #endregion

}
