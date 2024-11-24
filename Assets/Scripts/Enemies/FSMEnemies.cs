using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMEnemies : MonoBehaviour
{
    public enum State
    {
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
        state = State.Patrol;

        // Obtener el componente del script de detección
        playerDetection = GetComponent<PlayerDetection>();
        // Asegurar que el script está activo solo si el estado es Patrol
        UpdateDetectionState();
    }

    void Update()
    {
        StateBehaviour behaviour = GetComponent(state.ToString()) as StateBehaviour;
        behaviour.Behaviour();
        UpdateDetectionState();
    }

    void UpdateDetectionState()
    {
        // Solo activar PlayerDetection si el estado es Patrol
        if (playerDetection != null)
        {
            playerDetection.enabled = (state == State.Patrol);
        }
    }
}
