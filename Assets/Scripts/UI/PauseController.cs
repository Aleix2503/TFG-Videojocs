using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PauseController : MonoBehaviour
{
    private PlayerController playerController;
    public GameObject pauseMenu;
    private PlayerInputActions m_playerControls;
    private InputAction inputAction_pause;

    private bool isPaused = false;

    private void Awake()
    {
        m_playerControls = new PlayerInputActions();
        playerController = FindObjectOfType<PlayerController>();
    }

    private void OnEnable()
    {
        // Initialize and enable the pause input action
        inputAction_pause = m_playerControls.Player.Pause;
        inputAction_pause.Enable();
        inputAction_pause.performed += _ => TogglePause();
    }

    private void OnDisable()
    {
        inputAction_pause.Disable();
        inputAction_pause.performed -= _ => TogglePause();
    }

    private void TogglePause()
    {
        if (!isPaused)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        isPaused = true;
        playerController.DisablePlayerControls();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        isPaused = false;
        playerController.EnablePlayerControls();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void SetSelectedButton(GameObject button)
    {
        EventSystem.current.SetSelectedGameObject(button);
    }
}

