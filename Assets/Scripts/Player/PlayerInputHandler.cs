using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public PlayerInputActions m_playerControls;

    public float movementInput { get; private set; }
    public bool jumpInput { get; private set; }

    private InputAction inputAction_move;
    private InputAction inputAction_jump;

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
    }

    private void OnDisable()
    {
        inputAction_move.Disable();
        inputAction_jump.Disable();
    }

    private void Update()
    {
        movementInput = inputAction_move.ReadValue<float>();
        jumpInput = inputAction_jump.ReadValue<float>() > 0.5f ? true : false;
    }
}
