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
    public int absoluteMovementInput { get; private set; }
    public bool jumpInput { get; private set; }
    public bool jumpInputHeld { get; private set; }

    public bool dashInput { get; private set; }

    public bool bubbleInput { get; private set; }

    public bool expandInput { get; private set; }

    public PlayerInputActions m_playerControls;

    private InputAction inputAction_move;
    private InputAction inputAction_jump;
    private InputAction inputAction_dash;
    private InputAction inputAction_bubble;
    private InputAction inputAction_expand;

    private void Awake()
    {
        m_playerControls = new PlayerInputActions();
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

        if (inputAction_jump.WasPressedThisFrame())
        {
            jumpInput = true;
        }

        jumpInputHeld = inputAction_jump.ReadValue<float>() > 0.5f ? true : false;

        dashInput = inputAction_dash.WasPressedThisFrame();
        bubbleInput = inputAction_bubble.WasPressedThisFrame();
        expandInput = inputAction_expand.WasReleasedThisFrame();
    }

    public void UseJumpInput() => jumpInput = false;
    public void UseDashInput() => dashInput = false;
    public void UseBubbleInput() => bubbleInput = false;
    public void UseExpandInput() => expandInput = false;
}
