using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMEnemies : MonoBehaviour
{
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

    private void OnEnable()
    {
        life = maxLife;
    }

    void Start()
    {
        EnemyManager.Instance?.RegisterEnemy(gameObject);

        initialState = state;
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

    public void Heal()
    {
        life = maxLife;
    }

    public void Revive()
    {
        state = initialState;
        GetComponent<HitRoomba>()?.setAlreadyHit();
        GetComponent<Recover>()?.setAlreadyCalled();
        GetComponent<AttackTonto>()?.setAlreadyAttacked();
        GetComponent<DieRoomba>()?.setAlredyDead();
        GetComponent<Idle>()?.setAlreadyIdleing();
        GetComponent<SuckRoomba>()?.setAlreadySucking();
    }
}
