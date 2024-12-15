using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMEnemies : MonoBehaviour
{
    [SerializeField]
    public int life;

    public enum State
    {
        Idle,
        Patrol,
        Alert,
        Attack,
        Recover,
        Hit,
        Die
    }

    [SerializeField]
    public State state;

    // Referencia al script de detección
    private PlayerDetection playerDetection;

    void Start()
    {
        // Obtener el componente del script de detección
        playerDetection = GetComponent<PlayerDetection>();
        // Asegurar que el script está activo solo si el estado es Patrol
        UpdateDetectionState();
    }

    void Update()
    {
        if (life <= 0) 
        { 
            state = State.Die;
        }

        StateBehaviour behaviour = GetComponent(state.ToString()) as StateBehaviour;
        behaviour.Behaviour();

        UpdateDetectionState();
    }

    void UpdateDetectionState()
    {
        // Solo activar PlayerDetection si el estado es Patrol o Alert o Idle
        if (playerDetection != null)
        {
            playerDetection.enabled = (state == State.Patrol || state == State.Alert || state == State.Idle);
        }
    }
}
