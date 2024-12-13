using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    /// <summary>
    /// Acts as a middleman between the new input system and PlayerController.
    /// PlayerStates read from these public variables to interpret the current player inputs.
    /// </summary>

    public float rawMovementInput { get; private set; }
    private float rawAtackDirection;
    public int absoluteMovementInput { get; private set; }
    public int absoluteAttackDirection { get; private set; }
    public bool jumpInput { get; private set; }
    public bool jumpInputHeld { get; private set; }

    public bool dashInput { get; private set; }

    public bool bubbleInput { get; private set; }
    public bool bubbleInputHeld { get; private set; }

    public bool expandInput { get; private set; }

    private float lastJumpTime = 0f;

    public PlayerInputActions m_playerControls;
    private PlayerControlValues playerValues;

    private InputAction inputAction_move;
    private InputAction inputAction_jump;
    private InputAction inputAction_dash;
    private InputAction inputAction_bubble;
    private InputAction inputAction_expand;
    private InputAction inputAction_attackDirection;
    private InputAction inputAction_attack;

    private void Awake()
    {
        m_playerControls = new PlayerInputActions();
    }

    public void Initialize(PlayerControlValues playerValues)
    {
        this.playerValues = playerValues;
    }

    private void OnEnable()
    {
        inputAction_move = m_playerControls.Player.Move;
        inputAction_move.Enable();

        inputAction_jump = m_playerControls.Player.Jump;
        inputAction_jump.Enable();

        inputAction_dash = m_playerControls.Player.Dash;
        inputAction_dash.Enable();

        inputAction_bubble = m_playerControls.Player.Bubble;
        inputAction_bubble.Enable();

        inputAction_expand = m_playerControls.Player.Expand;
        inputAction_expand.Enable();

        inputAction_attackDirection = m_playerControls.Player.AttackDirection;
        inputAction_attackDirection.Enable();

        inputAction_attack = m_playerControls.Player.Attack;
        inputAction_attack.Enable();
    }

    private void OnDisable()
    {
        inputAction_move.Disable();
        inputAction_jump.Disable();
    }

    private void Update()
    {
        rawMovementInput = inputAction_move.ReadValue<float>();
        absoluteMovementInput = Mathf.RoundToInt(rawMovementInput);

        rawAtackDirection = inputAction_attackDirection.ReadValue<float>();
        absoluteAttackDirection = Mathf.RoundToInt(rawAtackDirection);

        if (inputAction_jump.WasPressedThisFrame())
        {
            jumpInput = true;
            lastJumpTime = 0;
        } else
        {
            lastJumpTime += Time.deltaTime;
            if (lastJumpTime > playerValues.jumpBufferTime)
            {
                jumpInput = false;
            }
        }

        jumpInputHeld = inputAction_jump.ReadValue<float>() > 0.5f ? true : false;
        bubbleInputHeld = inputAction_bubble.ReadValue<float>() > 0.5f ? true : false;

        dashInput = inputAction_dash.WasPressedThisFrame();
        bubbleInput = inputAction_bubble.WasPressedThisFrame();
        expandInput = inputAction_expand.WasPressedThisFrame();
    }

    public void EnablePlayerInput()
    {
        inputAction_move.Enable();
        inputAction_jump.Enable();
        inputAction_dash.Enable();
        inputAction_bubble.Enable();
        inputAction_expand.Enable();
        inputAction_attackDirection.Enable();
        inputAction_attack.Enable();
    }

    public void DisablePlayerInput()
    {
        inputAction_move.Disable();
        inputAction_jump.Disable();
        inputAction_dash.Disable();
        inputAction_bubble.Disable();
        inputAction_expand.Disable();
        inputAction_attackDirection.Disable();
        inputAction_attack.Disable();
    }
}
