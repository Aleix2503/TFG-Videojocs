using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMEnemies : MonoBehaviour
{
    [Header("Values")]
    [SerializeField]
    private int maxLife = 5;

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
    private State initialState;

    // Referencia al script de detección
    private PlayerDetection playerDetection;
    [Space]

    [Header("Die Values")]
    [SerializeField]
    private Vector2 offset;
    [SerializeField]
    private Vector2 dieBox;
    [SerializeField]
    private LayerMask hazardLayer;

    private Vector3 initialPosition;

    private void OnEnable()
    {
        life = maxLife;
    }

    void Start()
    {
        EnemyManager.Instance?.RegisterEnemy(gameObject);

        initialPosition = transform.position;
        initialState = state;
        // Obtener el componente del script de detección
        playerDetection = GetComponent<PlayerDetection>();
        // Asegurar que el script está activo solo si el estado es Patrol
        UpdateDetectionState();
    }

    void Update()
    {
        if (life <= 0 || checkIfTouchingHazard())
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

    public void Heal()
    {
        life = maxLife;
    }

    public void Revive()
    {
        transform.position = initialPosition;
        state = initialState;
        GetComponent<HitRoomba>()?.setAlreadyHit();
        GetComponent<Recover>()?.setAlreadyCalled();
        GetComponent<AttackTonto>()?.setAlreadyAttacked();
        GetComponent<DieRoomba>()?.setAlredyDead();
        GetComponent<Idle>()?.setAlreadyIdleing();
        GetComponent<SuckRoomba>()?.setAlreadySucking();
    }

    private bool checkIfTouchingHazard()
    {
        return Physics2D.OverlapBox((Vector2)transform.position + offset, dieBox, 0, hazardLayer);
    }

    private void OnDrawGizmos()
    {
        // Configura el color del Gizmo
        Gizmos.color = Color.red;

        // Calcula la posición del centro de la caja
        Vector2 boxCenter = (Vector2)transform.position + offset;

        // Dibuja la caja en el editor
        Gizmos.DrawWireCube(boxCenter, dieBox);
    }
}
