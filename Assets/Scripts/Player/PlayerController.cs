using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D m_rb2D;
    public PlayerInputActions m_playerControls;

    private InputAction move;
    private InputAction jump;

    float moveDirection;

    private void Awake()
    {
        m_playerControls = new PlayerInputActions();
    }

    private void OnEnable()
    {
        move = m_playerControls.Player.Move;
        move.Enable();

        jump = m_playerControls.Player.Jump;
        jump.Enable();

    }

    private void OnDisable()
    {
        move.Disable();
        jump.Disable();
    }

    void Start()
    {
        
    }


    void Update()
    {
        moveDirection = move.ReadValue<float>();
        print(moveDirection);
    }
}
